/**
 * SmartHR Chat Interface
 * Handles real-time communication with the SmartHR AI assistant
 */
document.addEventListener('DOMContentLoaded', () => {
    // DOM Elements
    const chatForm = document.getElementById('chat-form');
    const chatMessages = document.getElementById('chat-messages');
    const userInput = document.getElementById('user-input');
    const sendButton = document.querySelector('.send-button');
    const userName = abp.currentUser.userName;
    
    // State
    let isProcessing = false;

    /**
     * Initialize the chat interface
     */
    const initialize = () => {
        scrollToBottom();
        setupEventListeners();
        userInput.focus();
    };

    /**
     * Setup all event listeners
     */
    const setupEventListeners = () => {
        // Form submission handler
        chatForm.addEventListener('submit', handleSubmit);
        
        // Auto-resize textarea
        userInput.addEventListener('input', () => {
            userInput.style.height = 'auto';
            userInput.style.height = `${userInput.scrollHeight}px`;
        });
        
        // Handle Enter key (Shift+Enter for new line)
        userInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                
                if (!isProcessing) {
                    chatForm.dispatchEvent(new Event('submit'));
                }
            }
        });
    };

    /**
     * Handle form submission
     * @param {Event} e - Submit event
     */
    const handleSubmit = (e) => {
        e.preventDefault();

        // Prevent submission if already processing
        if (isProcessing) return;

        const message = userInput.value.trim();
        if (message === '') return;

        // Start processing sequence
        startProcessing();
        sendMessageToAPI(message);
        
        return false;
    };

    /**
     * Setup UI state for processing
     */
    const startProcessing = () => {
        disableSendButton();
        isProcessing = true;
    };

    /**
     * Reset UI state after processing
     */
    const endProcessing = () => {
        enableSendButton();
        isProcessing = false;
    };

    /**
     * Send message to API and handle response
     * @param {string} message - User message
     */
    const sendMessageToAPI = (message) => {
        // Add user message immediately with animation
        addMessage(userName, message, true);
        userInput.value = '';

        // Show typing indicator
        showTypingIndicator();

        // Call API
        wafi.smartHR.controllers.smartAI.ask(
            { question: message },
            {
                contentType: 'application/json',
                dataType: 'json'
            }
        )
        .then((response) => {
            removeTypingIndicator();
            addMessage('SmartHR', response.answer, false);
            endProcessing();
        })
        .catch((error) => {
            removeTypingIndicator();
            addMessage('SmartHR', "Sorry, there was an error processing your request.", false);
            console.error("API Error:", error);
            endProcessing();
        });
    };

    /**
     * Disable send button during processing
     */
    const disableSendButton = () => {
        sendButton.disabled = true;
        sendButton.style.opacity = '0.5';
        sendButton.style.cursor = 'not-allowed';
    };

    /**
     * Enable send button after processing
     */
    const enableSendButton = () => {
        sendButton.disabled = false;
        sendButton.style.opacity = '1';
        sendButton.style.cursor = 'pointer';
    };

    /**
     * Add a message to the chat
     * @param {string} sender - Message sender
     * @param {string} content - Message content
     * @param {boolean} isUser - Whether message is from user
     */
    const addMessage = (sender, content, isUser) => {
        const now = new Date();
        const timeString = now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

        // Create message container
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${isUser ? 'user-message' : 'ai-message'} fade-in`;
        
        // Create header elements
        const messageHeader = document.createElement('div');
        messageHeader.className = 'message-header';
        
        const messageSender = document.createElement('div');
        messageSender.className = 'message-sender';
        messageSender.textContent = sender;
        
        const messageTime = document.createElement('div');
        messageTime.className = 'message-time';
        messageTime.textContent = timeString;
        
        // Create content element
        const messageContent = document.createElement('div');
        messageContent.className = 'message-content';
        
        // Handle content differently based on sender
        if (isUser) {
            messageContent.textContent = content;
        } else {
            // Convert markdown-style bold formatting to HTML
            const formattedContent = content.replace(/\*\*(.*?)\*\*/g, '<b>$1</b>');
            messageContent.innerHTML = formattedContent;
        }
        
        // Assemble message components
        messageHeader.appendChild(messageSender);
        messageHeader.appendChild(messageTime);
        messageDiv.appendChild(messageHeader);
        messageDiv.appendChild(messageContent);

        // Add to chat
        const typingIndicator = document.querySelector('.typing-indicator-container');
        if (typingIndicator) {
            chatMessages.replaceChild(messageDiv, typingIndicator);
        } else {
            chatMessages.appendChild(messageDiv);
        }

        scrollToBottom();
    };

    /**
     * Show typing indicator while waiting for response
     */
    const showTypingIndicator = () => {
        const now = new Date();
        const timeString = now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

        const typingDiv = document.createElement('div');
        typingDiv.className = 'message ai-message typing-indicator-container';
        typingDiv.innerHTML = `
            <div class="message-header">
                <div class="message-sender">SmartHR</div>
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
    };

    /**
     * Remove typing indicator when response is received
     */
    const removeTypingIndicator = () => {
        const typingIndicator = document.querySelector('.typing-indicator-container');
        if (typingIndicator) {
            typingIndicator.remove();
        }
    };

    /**
     * Scroll chat to bottom
     */
    const scrollToBottom = () => {
        chatMessages.scrollTop = chatMessages.scrollHeight;
    };

    // Initialize the chat interface
    initialize();
});