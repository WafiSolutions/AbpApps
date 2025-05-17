/**
 * Workspace Selector Module
 * Handles workspace selection in the user menu
 */
(function () {
    'use strict';

    /**
     * Initialize the workspace selector functionality
     */
    function initWorkspaceSelector() {
        const workspaceSelector = $('#workspaceSelector');
        
        if (!workspaceSelector.length) return;
        
        // Load workspaces from API
        loadWorkspaces(workspaceSelector);
        
        // Set up change handler
        setupChangeHandler(workspaceSelector);
    }

    /**
     * Load workspaces from the API and populate the dropdown
     * @param {jQuery} selectorElement - The workspace selector element
     */
    function loadWorkspaces(selectorElement) {
        wafi.abp.workspaces.services.workspace.getAll({})
            .then(result => {
                if (result?.items?.length) {
                    populateOptions(selectorElement, result.items);
                    restoreSavedSelection(selectorElement);
                }
            })
            .catch(error => {
                // Silently handle error - could add proper error handling if required
            });
    }

    /**
     * Populate dropdown options with workspace data
     * @param {jQuery} selectorElement - The workspace selector element
     * @param {Array} workspaces - Array of workspace objects
     */
    function populateOptions(selectorElement, workspaces) {
        workspaces.forEach(workspace => {
            selectorElement.append(
                $('<option></option>')
                    .val(workspace.id)
                    .text(workspace.name)
            );
        });
    }

    /**
     * Restore previously selected workspace from localStorage
     * @param {jQuery} selectorElement - The workspace selector element
     */
    function restoreSavedSelection(selectorElement) {
        const savedWorkspaceId = localStorage.getItem('selectedWorkspaceId');
        if (savedWorkspaceId) {
            selectorElement.val(savedWorkspaceId);
        }
    }

    /**
     * Set up change event handler for workspace selection
     * @param {jQuery} selectorElement - The workspace selector element
     */
    function setupChangeHandler(selectorElement) {
        selectorElement.on('change', function() {
            const selectedWorkspaceId = $(this).val();
            if (selectedWorkspaceId) {
                localStorage.setItem('selectedWorkspaceId', selectedWorkspaceId);
                abp.notify.info('Workspace changed to: ' + $(this).find('option:selected').text());
            }
        });
    }

    /**
     * Wait for dependencies to be available before initializing
     */
    function waitForDependencies() {
        if (typeof window.jQuery !== 'undefined' && 
            typeof abp !== 'undefined' && 
            typeof wafi?.abp?.workspaces?.services?.workspace !== 'undefined') {
            
            // All dependencies available, initialize on document ready
            $(document).ready(initWorkspaceSelector);
        } else {
            // Wait for dependencies
            setTimeout(waitForDependencies, 50);
        }
    }

    // Start initialization process
    waitForDependencies();
})();