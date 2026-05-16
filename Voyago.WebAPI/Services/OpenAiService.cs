using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Voyago.WebAPI.Dtos;

namespace Voyago.WebAPI.Services;

public class OpenAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenAiService> _logger;

    private const string SystemPrompt = @"
You are Voyago AI, a friendly and knowledgeable travel assistant for the Voyago hotel platform.

Your expertise:
- Travel destinations, attractions, and local culture
- Hotel recommendations and travel tips
- Best time to visit specific places
- Currency, weather, language, and visa basics
- Itinerary planning and trip logistics

How to respond:
- Be concise: 2-4 short paragraphs maximum
- Use plain text, no markdown formatting (no **, no #, no bullet points with asterisks)
- Be warm and friendly, like a helpful concierge
- For travel questions: give specific, actionable advice
- For off-topic questions (math, coding, politics, etc.): briefly mention you're a travel assistant, then give a short helpful answer if possible and steer back to travel
- Never make up specific prices, addresses, or contact info you don't know
- If you don't know something, say so honestly

Always end with a brief, friendly question or suggestion to keep the conversation going.
";

    public OpenAiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<OpenAiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AIResponseDto> AskAsync(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return new AIResponseDto
            {
                Success = false,
                ErrorMessage = "Please ask a question."
            };
        }

        if (question.Length > 500)
        {
            return new AIResponseDto
            {
                Success = false,
                ErrorMessage = "Your question is too long. Please keep it under 500 characters."
            };
        }

        try
        {
            var apiKey = _configuration["OpenAi:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogError("OpenAI API key not configured");
                return new AIResponseDto
                {
                    Success = false,
                    ErrorMessage = "AI service is not configured. Please contact support."
                };
            }

            var requestPayload = new OpenAiRequestDto
            {
                Model = "gpt-4o-mini",  
                Messages = new List<OpenAiMessageDto>
                {
                    new() { Role = "system", Content = SystemPrompt },
                    new() { Role = "user", Content = question }
                },
                Temperature = 0.7,
                MaxTokens = 500
            };

            var json = JsonSerializer.Serialize(requestPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions")
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("OpenAI API returned {Status}: {Body}",
                    response.StatusCode, errorBody);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new AIResponseDto
                    {
                        Success = false,
                        ErrorMessage = "AI service authentication failed."
                    };
                }

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return new AIResponseDto
                    {
                        Success = false,
                        ErrorMessage = "Too many requests. Please wait a moment."
                    };
                }

                return new AIResponseDto
                {
                    Success = false,
                    ErrorMessage = "AI service is temporarily unavailable. Please try again."
                };
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var openAiResponse = JsonSerializer.Deserialize<OpenAiResponseDto>(responseJson);

            if (openAiResponse?.Error is not null)
            {
                _logger.LogError("OpenAI API error: {Message}", openAiResponse.Error.Message);
                return new AIResponseDto
                {
                    Success = false,
                    ErrorMessage = "AI service returned an error. Please try again."
                };
            }

            var answer = openAiResponse?.Choices?
                .FirstOrDefault()?
                .Message?.Content;

            if (string.IsNullOrWhiteSpace(answer))
            {
                _logger.LogWarning("OpenAI returned empty answer for: {Question}", question);
                return new AIResponseDto
                {
                    Success = false,
                    ErrorMessage = "I couldn't generate an answer. Please rephrase your question."
                };
            }

            if (openAiResponse?.Usage is not null)
            {
                _logger.LogInformation(
                    "OpenAI tokens: prompt={Prompt}, completion={Completion}, total={Total}",
                    openAiResponse.Usage.PromptTokens,
                    openAiResponse.Usage.CompletionTokens,
                    openAiResponse.Usage.TotalTokens);
            }

            return new AIResponseDto
            {
                Success = true,
                Answer = answer.Trim()
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "OpenAI HTTP request failed");
            return new AIResponseDto
            {
                Success = false,
                ErrorMessage = "Network error. Please check your connection and try again."
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "OpenAI response parsing failed");
            return new AIResponseDto
            {
                Success = false,
                ErrorMessage = "AI response was invalid. Please try again."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in OpenAI service");
            return new AIResponseDto
            {
                Success = false,
                ErrorMessage = "Something went wrong. Please try again."
            };
        }
    }
}