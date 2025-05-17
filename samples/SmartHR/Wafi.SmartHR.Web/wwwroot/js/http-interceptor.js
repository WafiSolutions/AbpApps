/**
 * HTTP Request Interceptor Module
 * @description Adds the selected workspace ID as a header to outgoing HTTP requests
 * @version 1.0.0
 */
(function () {
    'use strict';

    // Reference to the shared constants
    const CONFIG = window.WORKSPACE_CONSTANTS || {
        STORAGE_KEY: 'selectedWorkspaceId',
        HEADER_NAME: 'X-Workspace-Id'
    };

    /**
     * Add workspace header to any XHR object
     * @param {XMLHttpRequest} xhr - The XHR object to modify
     * @private
     */
    function _addWorkspaceHeader(xhr) {
        const workspaceId = localStorage.getItem(CONFIG.STORAGE_KEY);
        if (workspaceId) {
            xhr.setRequestHeader(CONFIG.HEADER_NAME, workspaceId);
        }
    }

    /**
     * Override jQuery ajax requests
     * @private
     */
    function initInterceptors() {
        if (!window.jQuery) return;

        jQuery.ajaxPrefilter(function (options, originalOptions, jqXHR) {
            _addWorkspaceHeader(jqXHR);
        });
    }


    /**
     * Wait for required dependencies before initializing
     */
    function waitForDependencies() {
        if (typeof abp !== 'undefined' && abp.ajax) {
            initInterceptors();
        } else if (typeof window.jQuery !== 'undefined') {
            initInterceptors();
        } else if (typeof window.fetch !== 'undefined') {
            initInterceptors();
        } else {
            setTimeout(waitForDependencies, 50);
        }
    }

    // Start initialization process
    waitForDependencies();
})(); 
