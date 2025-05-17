/**
 * HTTP Request Interceptor Module
 * @description Adds the selected workspace ID as a header to outgoing HTTP requests
 * @version 1.0.0
 */
(function () {
    'use strict';

    // Configuration
    const CONFIG = Object.freeze({
        STORAGE_KEY: 'selectedWorkspaceId',
        HEADER_NAME: 'X-Workspace-Id'
    });

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
     * Add workspace header to fetch request options
     * @param {Object} options - The fetch request options
     * @private
     */
    function _addWorkspaceHeaderToFetchOptions(options) {
        const workspaceId = localStorage.getItem(CONFIG.STORAGE_KEY);
        if (!workspaceId) return;

        options.headers = options.headers || {};
        
        if (options.headers instanceof Headers) {
            options.headers.append(CONFIG.HEADER_NAME, workspaceId);
        } else {
            options.headers[CONFIG.HEADER_NAME] = workspaceId;
        }
    }

    /**
     * Override Fetch API
     * @private
     */
    function _interceptFetch() {
        if (!window.fetch) return;
        
        const originalFetch = window.fetch;
        
        window.fetch = function(resource, options = {}) {
            _addWorkspaceHeaderToFetchOptions(options);
            return originalFetch(resource, options);
        };
    }

    /**
     * Override ABP ajax requests
     * @private
     */
    function _interceptAbpAjax() {
        if (!(typeof abp !== 'undefined' && abp.ajax)) return;
        
        const originalOnBeforeSend = abp.ajax.onBeforeSend;
        
        abp.ajax.onBeforeSend = function(xhr) {
            if (typeof originalOnBeforeSend === 'function') {
                originalOnBeforeSend(xhr);
            }
            
            _addWorkspaceHeader(xhr);
        };
    }

    /**
     * Override jQuery ajax requests
     * @private
     */
    function _interceptJQueryAjax() {
        if (!window.jQuery) return;
        
        jQuery.ajaxPrefilter(function(options, originalOptions, jqXHR) {
            _addWorkspaceHeader(jqXHR);
        });
    }

    /**
     * Initialize all HTTP interceptors
     */
    function initInterceptors() {
        _interceptFetch();
        _interceptAbpAjax();
        _interceptJQueryAjax();
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
