/**
 * HTTP Request Interceptor
 * Adds the selected workspace ID as a header to outgoing requests
 */
(function () {
    'use strict';

    // Constants
    const WORKSPACE_STORAGE_KEY = 'selectedWorkspaceId';
    const WORKSPACE_HEADER_NAME = 'X-Workspace-Id';

    /**
     * Initialize the HTTP interceptor
     */
    debugger
    function initInterceptor() {
        debugger
        // Hook into ABP's AJAX interceptor
        abp.ajax.onBeforeSend = function (xhr) {
            const workspaceId = localStorage.getItem(WORKSPACE_STORAGE_KEY);
            // Only add header if a workspace is selected
            if (workspaceId) {
                xhr.setRequestHeader(WORKSPACE_HEADER_NAME, workspaceId);
            }
        };
    }

    /**
     * Wait for dependencies to be available before initializing
     */
    function waitForDependencies() {
        if (typeof abp !== 'undefined' && abp.ajax) {
            initInterceptor();
        } else {
            setTimeout(waitForDependencies, 50);
        }
    }

    // Start initialization process
    waitForDependencies();
})(); 