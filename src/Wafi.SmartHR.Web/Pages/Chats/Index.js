// Index.js
document.addEventListener('DOMContentLoaded', function () {
    const chatForm = document.getElementById('chat-form');
    const chatMessages = document.getElementById('chat-messages');
    const userInput = document.getElementById('user-input');
    const sendButton = document.querySelector('.send-button');
    const userName = abp.currentUser.userName;
    let isProcessing = false;

    // Scroll to bottom initially
    scrollToBottom();

    chatForm.addEventListener('submit', function (e) {
        e.preventDefault();

        // Prevent submission if already processing
        if (isProcessing) return;

        const message = userInput.value.trim();
        if (message === '') return;

        // Disable send button while processing
        disableSendButton();
        isProcessing = true;

        // Add user message immediately with animation
        addMessage(userName, message, true);
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
            enableSendButton();
            isProcessing = false;
        }).catch(function (error) {
            removeTypingIndicator();
            addMessage('AI', "Sorry, there was an error processing your request.", false);
            console.error("API Error:", error);
            enableSendButton();
            isProcessing = false;
        });

        return false;
    });

    function disableSendButton() {
        sendButton.disabled = true;
        sendButton.style.opacity = '0.5';
        sendButton.style.cursor = 'not-allowed';
    }

    function enableSendButton() {
        sendButton.disabled = false;
        sendButton.style.opacity = '1';
        sendButton.style.cursor = 'pointer';
    }

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

    // Auto-resize textarea
    userInput.addEventListener('input', function() {
        this.style.height = 'auto';
        this.style.height = (this.scrollHeight) + 'px';
    });

    // Handle Enter key (Shift+Enter for new line)
    userInput.addEventListener('keydown', function(e) {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            
            // Only dispatch submit event if not currently processing
            if (!isProcessing) {
                chatForm.dispatchEvent(new Event('submit'));
            }
        }
    });

    // Focus textarea on page load
    userInput.focus();
});