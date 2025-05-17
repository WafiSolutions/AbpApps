// Define a function to initialize once jQuery is available
function initializeWhenJQueryReady() {
    console.log('Workspace selector script loaded');
    
    // Make sure wafi.abp.workspaces.services.workspace is defined
    var checkApiAvailable = function() {
        if (typeof wafi !== 'undefined' && 
            wafi.abp && 
            wafi.abp.workspaces && 
            wafi.abp.workspaces.services && 
            wafi.abp.workspaces.services.workspace) {
            
            initWorkspaceSelector();
        } else {
            console.log('API not available yet, retrying in 500ms');
            setTimeout(checkApiAvailable, 500);
        }
    };
    
    var initWorkspaceSelector = function() {
        console.log('Initializing workspace selector');
        var workspaceSelector = $('#workspaceSelector');
        
        if (workspaceSelector.length === 0) {
            console.error('Workspace selector element not found');
            return;
        }
        
        console.log('Calling workspace API');
        
        // Load workspaces
        wafi.abp.workspaces.services.workspace.getAll({})
            .then(function(result) {
                console.log('API result:', result);
                if (result && result.items && result.items.length > 0) {
                    result.items.forEach(function(workspace) {
                        workspaceSelector.append(
                            $('<option></option>')
                                .val(workspace.id)
                                .text(workspace.name)
                        );
                    });
                    
                    // Set previously selected workspace if exists
                    var savedWorkspaceId = localStorage.getItem('selectedWorkspaceId');
                    if (savedWorkspaceId) {
                        workspaceSelector.val(savedWorkspaceId);
                    }
                } else {
                    console.log('No workspaces found or empty result');
                }
            })
            .catch(function(error) {
                console.error('Error loading workspaces:', error);
            });

        // Handle workspace change
        workspaceSelector.on('change', function() {
            var selectedWorkspaceId = $(this).val();
            if (selectedWorkspaceId) {
                // Store the selected workspace ID in localStorage
                localStorage.setItem('selectedWorkspaceId', selectedWorkspaceId);
                // You can add additional logic here to handle workspace change
                abp.notify.info('Workspace changed to: ' + $(this).find('option:selected').text());
            }
        });
    };
    
    // Start checking if API is available
    checkApiAvailable();
}

// Check if jQuery is available, if not wait for it
(function checkJQuery() {
    if (typeof window.jQuery !== 'undefined') {
        // jQuery is loaded, initialize
        jQuery(document).ready(function() {
            initializeWhenJQueryReady();
        });
    } else {
        // jQuery is not loaded yet, wait and try again
        console.log('jQuery not available yet, waiting...');
        setTimeout(checkJQuery, 100);
    }
})();