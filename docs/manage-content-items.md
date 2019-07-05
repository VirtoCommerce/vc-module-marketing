# Manage Dynamic Content

## Create Content Items

Creating a **Dynamic Content** in VirtoCommerce platform manager requires that the user provides a name for the Dynamic Content, selects a Content type and provides values for the type’s parameters.

1. The user opens the Marketing Module and selects **‘Dynamic Content’**;
1. The system will display the ‘Dynamic Content’ screen with the following list:  
     1. Dynamic items;
     1. Content placeholders;
     1. Content publishing.  
1. The user selects **‘Content items’**;
1. The ‘Content items’ screen with the existing list of Content items will be displayed;  
     1. If no Content items were previously added, the ‘Content items’ screen will be empty.
1. The user clicks the ‘Add’ button on the top of the screen;
1. The system will open the ‘New content items element’ screen with possibility to create a ‘Content items folder’ in case the user creates the Content items for the very first time AND add a new content item;
     1. If the user creates the Content item for the first time, then he should select the ‘Content items folder’ option. This folder will serve as a container for the content items;
     1. The user should enter the folder name and description and click the ‘Create’ button.
1. The system will create the new content folder, which will be displayed on the ‘Content items’ screen;
1. Once the Content folder is created, the user can start adding Content items. In order to add a new Content item, the user should click the ‘Add’ button and select ‘Content item’;
1. The system will display the ‘New content item element’ screen:
     1. ‘Content item name’ input field – required;
     1. ‘Content item description’ text field- required;
     1. ‘Content type’ drop down optional;
     1. The ‘Create’ button disabled if the required fields are empty.  
1. The user fills out the required fields, selects the Content type (if needed) and clicks the ‘Create’ button;
1. The system will create the Content item and display it on the list.  

![Fig. Dynamic Content](media/screen-dynamic-content.png)

![Fig. Add Content Item](media/screen-add-content-item.png)

### Edit Content Type

1. The user clicks on the ‘Edit’ button next to the ‘Content type’;
1. The system will display the ‘Dictionary values’ screen;
1. The user enters the Dictionary entry name into the correspondent input text field and click the ‘Add’ button;
1. The new value will be added and displayed on the ‘Current values’ list and under the Content type’ drop down.

![Edit Content Type](media/screen-edit-content-type.png)

## Edit Content Item

1. In order to edit the Dynamic Content, the user should select the ‘Marketing’ Module, navigate to Dynamic Content->Content items and select the Content item he wants to edit;
1. The system will display the ‘Edit content item element’ screen;
1. The user makes the editing needed and clicks ‘Save’;
1. The system will save the changes made.  

**Important**: If the user edits also the Content type of the Dynamic Content, the sensitive properties of the item will be changed too!

![Fig. Edit Content Item](media/screen-edit-content-item.png)

## Delete Content Item

1. The user selects ‘Marketing’ Module, navigates to Dynamic content-> Content items and selects the Content item he wants to delete;
1. The system will open the ‘Edit content item element’ screen;
1. The user clicks the ‘Delete’ button;
1. The system will display a deletion confirmation ‘Are you sure you want to delete this content item?’- ‘Yes’, ‘No’, ‘Cancel’;
1. The user selects the ‘Yes’ option and the system will delete the content item.  

**Important**: Only the Dynamic Content that doesn't belong to a Content publishing can be deleted otherwise the system will display a message informing the user that the associated Content Publishing should be deleted first.

![Fig. Delete Content Item](media/screen-delete-content-item.png)

![Fig. Confirm Deletion](media/screen-confirm-deletion.png)