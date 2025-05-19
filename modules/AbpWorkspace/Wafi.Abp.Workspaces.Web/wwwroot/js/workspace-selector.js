/**
 * Workspace Selector Module
 * Handles workspace selection in the user menu
 */
(function () {
    'use strict';

    // Reference to the shared constants
    const CONFIG = window.WORKSPACE_CONSTANTS || {
        STORAGE_KEY: 'selectedWorkspaceId',
        WORKSPACE_CHANGED: 'workspaceChanged',
        WORKSPACE_NAME: 'workspaceName'
    };

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
        
        // Show notification if workspace was just changed
        showNotificationIfWorkspaceChanged();
    }

    function showWorkspaceName() {
        const workspaceSelector = document.getElementById('workspaceSelector');
        const selectedWorkspaceName = document.getElementById('selectedWorkspaceName');

        if (workspaceSelector && selectedWorkspaceName) {
            // Set initial value
            const selectedOption = workspaceSelector.options[workspaceSelector.selectedIndex];
            if (selectedOption) {
                selectedWorkspaceName.textContent = selectedOption.text;
            }

            // Listen for changes
            workspaceSelector.addEventListener('change', function() {
                const selectedOption = this.options[this.selectedIndex];
                if (selectedOption) {
                    selectedWorkspaceName.textContent = selectedOption.text;
                }
            });
        }
    }
    


    /**
     * Show notification if workspace was changed before page reload
     */
    function showNotificationIfWorkspaceChanged() {
        if (localStorage.getItem(CONFIG.WORKSPACE_CHANGED) === 'true') {
            const workspaceName = localStorage.getItem(CONFIG.WORKSPACE_NAME);
            if (workspaceName) {
                // Use the localized format string
                const message = abp.localization.getResource('Workspace')('WorkspaceChanged').replace('{0}', workspaceName);
                abp.notify.info(message);
            }
            // Clear the flags
            localStorage.removeItem(CONFIG.WORKSPACE_CHANGED);
            localStorage.removeItem(CONFIG.WORKSPACE_NAME);
        }
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
                    // Show workspace name after workspaces are loaded
                    showWorkspaceName();
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
        const savedWorkspaceId = localStorage.getItem(CONFIG.STORAGE_KEY);
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
                const workspaceName = $(this).find('option:selected').text();
                
                // Store data for after reload
                localStorage.setItem(CONFIG.STORAGE_KEY, selectedWorkspaceId);
                localStorage.setItem(CONFIG.WORKSPACE_CHANGED, 'true');
                localStorage.setItem(CONFIG.WORKSPACE_NAME, workspaceName);
                
                // Reload page immediately
                location.reload();
            }
        });
    }

    /**
     * Wait for dependencies to be available before initializing
     */
    function waitForDependencies() {
        if (typeof window.jQuery !== 'undefined' &&
            typeof abp !== 'undefined' &&
            typeof window.wafi !== 'undefined' &&
            typeof wafi.abp?.workspaces?.services?.workspace !== 'undefined') {

            $(document).ready(initWorkspaceSelector);
        } else {
            setTimeout(waitForDependencies, 50);
        }
    }

    // Start initialization process
    waitForDependencies();
})(); 