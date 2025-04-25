// Index.js
document.addEventListener('DOMContentLoaded', function () {
    const chatForm = document.getElementById('chat-form');
    const chatMessages = document.getElementById('chat-messages');
    const userInput = document.getElementById('user-input');

    // Scroll to bottom initially
    scrollToBottom();

    chatForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const message = userInput.value.trim();
        if (message === '') return;

        // Add user message immediately with animation
        addMessage('User', message, true);
        userInput.value = '';

        // Show typing indicator
        showTypingIndicator();

        // Call your API
        wafi.smartHR.controllers.smartAI.ask({
            question: message
        }, {
            contentType: 'application/json',
            dataType: 'json'
        }).then(function (response) {
            removeTypingIndicator();
            addMessage('AI', response.answer, false);
        }).catch(function (error) {
            removeTypingIndicator();
            addMessage('AI', "Sorry, there was an error processing your request.", false);
            console.error("API Error:", error);
        });

        return false;
    });

    function addMessage(sender, content, isUser) {
        const now = new Date();
        const timeString = now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${isUser ? 'user-message' : 'ai-message'} fade-in`;
        messageDiv.innerHTML = `
            <div class="message-header">
                <div class="message-sender">${sender}</div>
                <div class="message-time">${timeString}</div>
            </div>
            <div class="message-content">${content}</div>
        `;

        const typingIndicator = document.querySelector('.typing-indicator-container');
        if (typingIndicator) {
            chatMessages.replaceChild(messageDiv, typingIndicator);
        } else {
            chatMessages.appendChild(messageDiv);
        }

        scrollToBottom();
    }

    function showTypingIndicator() {
        const now = new Date();
        const timeString = now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

        const typingDiv = document.createElement('div');
        typingDiv.className = 'message ai-message typing-indicator-container';
        typingDiv.innerHTML = `
            <div class="message-header">
                <div class="message-sender">AI</div>
                <div class="message-time">${timeString}</div>
            </div>
            <div class="message-content">
                <div class="typing-indicator">
                    <div class="typing-dot"></div>
                    <div class="typing-dot"></div>
                    <div class="typing-dot"></div>
                </div>
            </div>
        `;

        chatMessages.appendChild(typingDiv);
        scrollToBottom();
    }

    function removeTypingIndicator() {
        const typingIndicator = document.querySelector('.typing-indicator-container');
        if (typingIndicator) {
            typingIndicator.remove();
        }
    }

    function scrollToBottom() {
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }
});