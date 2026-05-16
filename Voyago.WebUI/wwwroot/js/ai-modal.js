// =========================================
// Voyago AI Modal — Open, close, ask
// =========================================

(function () {
    'use strict';

    // ---- DOM elements ----
    const modal = document.getElementById('ai-modal');
    const modalBody = document.getElementById('ai-modal-body');
    const form = document.getElementById('ai-modal-form');
    const input = document.getElementById('ai-input');
    const submitBtn = document.getElementById('ai-submit-btn');
    const spinner = submitBtn?.querySelector('.ai-spinner');
    const submitText = submitBtn?.querySelector('.ai-submit-text');
    const submitIcon = submitBtn?.querySelector('.ai-submit-icon');

    if (!modal || !form) return;

    // ---- Open modal ----
    function openModal(prefill) {
        modal.hidden = false;
        document.body.style.overflow = 'hidden';  // background scroll'u kilitle

        // Prefill text varsa input'a yaz ve auto-submit
        if (prefill) {
            input.value = prefill;
            setTimeout(() => form.requestSubmit(), 100);
        } else {
            setTimeout(() => input.focus(), 50);
        }
    }

    // ---- Close modal ----
    function closeModal() {
        modal.hidden = true;
        document.body.style.overflow = '';
    }

    // ---- Event: Open buttons ----
    document.querySelectorAll('[data-ai-open]').forEach(btn => {
        btn.addEventListener('click', () => openModal());
    });

    // ---- Event: Example chips (prefill + open) ----
    document.querySelectorAll('[data-ai-example]').forEach(chip => {
        chip.addEventListener('click', () => {
            const question = chip.getAttribute('data-ai-example');
            openModal(question);
        });
    });

    // ---- Event: Close buttons (overlay + X) ----
    document.querySelectorAll('[data-ai-close]').forEach(el => {
        el.addEventListener('click', closeModal);
    });

    // ---- Event: ESC key closes modal ----
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape' && !modal.hidden) {
            closeModal();
        }
    });

    // ---- Event: Cmd/Ctrl+Enter to submit ----
    input?.addEventListener('keydown', (e) => {
        if ((e.metaKey || e.ctrlKey) && e.key === 'Enter') {
            e.preventDefault();
            form.requestSubmit();
        }
    });

    // ---- Submit handler ----
    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const question = input.value.trim();
        if (!question) return;

        // Append user message to chat
        appendMessage('user', question);

        // Clear input + lock submit
        input.value = '';
        setLoading(true);

        try {
            const response = await fetch('/Ai/Ask', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ question })
            });

            const data = await response.json();

            if (data.success && data.answer) {
                appendMessage('assistant', data.answer);
            } else {
                appendMessage('error', data.errorMessage || 'Something went wrong. Please try again.');
            }
        } catch (err) {
            console.error('AI request failed:', err);
            appendMessage('error', 'Network error. Please check your connection.');
        } finally {
            setLoading(false);
            input.focus();
        }
    });

    // ---- Helper: append message bubble ----
    function appendMessage(role, text) {
        // Welcome mesajını ilk message'da kaldır
        const welcome = modalBody.querySelector('.ai-welcome');
        if (welcome) welcome.remove();

        const bubble = document.createElement('div');
        bubble.className = `ai-message ai-message-${role}`;

        if (role === 'user') {
            bubble.textContent = text;
        } else if (role === 'assistant') {
            // Plain text — system prompt no-markdown garantisi var
            // Yine de paragraf bölmesi için \n\n → <p> dönüşümü
            const paragraphs = text.split(/\n\n+/);
            paragraphs.forEach(p => {
                const para = document.createElement('p');
                para.textContent = p.replace(/\n/g, ' ');
                bubble.appendChild(para);
            });
        } else {
            bubble.textContent = text;
        }

        modalBody.appendChild(bubble);

        modalBody.scrollTop = modalBody.scrollHeight;
    }

    function setLoading(isLoading) {
        submitBtn.disabled = isLoading;
        input.disabled = isLoading;

        if (isLoading) {
            submitText.textContent = 'Thinking…';
            submitIcon.hidden = true;
            spinner.hidden = false;
        } else {
            submitText.textContent = 'Ask';
            submitIcon.hidden = false;
            spinner.hidden = true;
        }
    }
})();