/**
 * Workspace HTTP Interceptor
 * Automatically includes the workspace id in API requests
 */
(function () {
    'use strict';
    
    const CONFIG = window.WORKSPACE_CONSTANTS || {
        STORAGE_KEY: 'selectedWorkspaceId',
        HEADER_NAME: 'X-Workspace-Id'
    };
    
    // Wait for ABP to be ready
    document.addEventListener('abp.dynamicScriptsInitialized', function () {
        // Add workspace header to all AJAX requests
        abp.ajax.onBeforeSend = function (xhr) {
            // Get the workspace ID from local storage
            const workspaceId = localStorage.getItem(CONFIG.STORAGE_KEY);
            if (workspaceId) {
                xhr.setRequestHeader(CONFIG.HEADER_NAME, workspaceId);
            }
        };
    });
})(); 
