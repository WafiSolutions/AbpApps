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

    function getWorkspaceId() {
        return localStorage.getItem(CONFIG.STORAGE_KEY);
    }

    function initJQueryInterceptor() {
        if (!window.jQuery) return;

        jQuery(document).ajaxSend(function (event, xhr, settings) {
            const workspaceId = getWorkspaceId();
            const url = settings.url || '';

            if (workspaceId && (url.includes('/api/') || url.includes('/AbpApi'))) {
                xhr.setRequestHeader(CONFIG.HEADER_NAME, workspaceId);
            }
        });

    }

    function initialize() {
        initJQueryInterceptor();
    }

    // Trigger initialization immediately and on key events
    initialize();

    document.addEventListener('abp.dynamicScriptsInitialized', initialize);
    document.addEventListener('DOMContentLoaded', initialize);
    window.addEventListener('load', initialize);

})();
