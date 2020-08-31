# Overview

VirtoCommerce.Marketing module represents a marketing management system.

## Key Features

* Dynamic content management for storefront;
* Content publishing management;
* Promotions.

## Dynamic Content

The Dynamic Content feature allows marketers to create personalized shopping experiences for customers. Using this feature, the marketer can deliver elements of a shopping experience based on a set of conditions or context.

The following should be considered before determining under which conditions the Dynamic Content will be displayed to the customer:

* What type of content should be displayed;
* The target audience who will see the content;
* Time period when the content should be displayed;
* Where should the content be displayed.

The Dynamic Content is configured using the Virto Commerce platform manager so that the marketing personnel can set up Dynamic Content without relying on developers and server administrators. For images and Flash animations, they may need a graphic designer to create the file and upload it to the Assets. However, once that's done, the marketer can configure rules for displaying the content without any assistance from the technical team side.

The Dynamic Content consists of four independent components:

* **Dynamic Content**
* **Content Publishing**
* **Content Type**
* **Content Placeholder**

The first two components, **Dynamic Content** and **Content Publishing**, are set up via the Virto Commerce platform manager by a marketer. The Dynamic Content provides a name and a description of the type of content that will be displayed. The Content Publishing object specifies when, under what conditions and in which placeholder the Dynamic Content should be displayed.

The **Content Placeholder** is also created via VirtoCommerce platform manager, but they are created by developers, web designers and system management personnel. As soon as the  web developer adds the Content Place to a template, the developer or another user with appropriate permissions must register it in VirtoCommerce platform manager.

The **Content type** is a template used to define the Dynamic Content Type. It describes how a particular content type is displayed and what information is required for it to be displayed. VirtoCommerce includes several Content types for the most common types of Dynamic Content. For example, there is a Content type to display images with links (banner). Another one to display product data (product with image and price).

The following Dynamic **Content Types** are shipped with Virto Commerce out-of-the-box. Each of them requires one or more parameter values to specify things such as: what image file should be displayed or what the target web page should display after clicking on the element.


| Content type | Description | Parameters |
|--------------|-------------|------------|
|Flash | Displays an animated Flash file that cycles through three images. This Flash animation shows each of three images in succession, each having a clickable link that shows the shopper a different promotion. You will need to specify the URLs for each link within the Flash file.| FlashFilePath - path to the Flash animation file, Link1Url, Link2Url, Link3Url - enter the complete URL to the target page (item, promotion etc)|
| Html | Displays HTML content | RawHtml - enter raw Html formatted text here |
|ImageClickable |Displays an image that can be clicked to perform some action. For example, redirect to another page, product or promotion |Alternative text - text if the image can not be displayed. ImageUrl - link to the image. TargetUrl - link to the target when image clicked. Title - text (optional) |
| ImageNonClickable |Displays an image. For example, use this type to alert customers to a shopping cart promotion that will give them a discount on checkout |Alternative text - text if image can not be displayed. ImageFilePath - path to the image file |
|ProductsWithinCategory |Displays products of the category as slideshow |Category code - code of the category. Title - user friendly title of the category. Item count - how many items will be presented in the slideshow. New items - switch to show only new items.|

### Tags

The Dynamic Content functionality can be used to show specific content to targeted customers. The system determines the customers properties via Tags. VirtoCommerce incorporates tagging system that is used to set and evaluate tags that the user can use to segment Customers, and to decide when, where and to which Customer this content should be displayed.

The tagging data about a customer is captured in a variety of ways, including the following:

1. Information provided in a registered customer account;
1. Information entered by a Customer Service about a customer during the customer's service call;
1. Target and referring URLs and search terms captured when the user clicks a link that brings them to a storefront;
1. Customer Geo location.

Information captured by the tags is stored in the user’s session while he shops in on storefront. The information can then be used to evaluate whether the customer meets the conditions set for displaying the Dynamic Content. Tag information is captured only when the customer first visits the storefront during the browser session.

A Marketer or other Manager user with granted Dynamic Content permissions, uses the tags when creating conditions that determine whether a customer will see a piece of Dynamic Content or not.

[Publishing Conditions](/docs/publishing-conditions.md)

### Work with Dynamic Content

Once created, the Dynamic Content can be reused as many times as required in Content Publishing items.

Creating **Dynamic Content** in VirtoCommerce management application specifies what Content to deliver (e.g. Image, Clickable image, Flash etc.) and setting up its parameters.

Creating **Content Publishing** specifies which Dynamic Content(s) to view and under which conditions the Dynamic Content should be available.

**Important**: Dynamic Content won't be available on frontend until it's assigned to any of the Content Publishing item.

[Manage Content Items](/docs/manage-content-items.md)

[Manage Content Placeholders](/docs/manage-content-placeholders.md)

[Manage Content Publishing](/docs/manage-content-publishing.md)

### Add Advertising Spot

In order to add an advertising spot via Marketing Module, the user should first prepare the dynamic content infrastructure and go through the following steps:  

1. Create a content item.
1. Create a content placeholder.
1. Create a content publishing.  

Once the Dynamic content infrastructure is prepared, the user should add the dynamic content to a web site:  

1. The user should open the web page HTML file;
1. Insert the code into the HTML file. The ID is the placeholder name :
`<vc-content-place id="Right banner 240x400" class="col-sm-4 col-md-4 rightblock"></vc-content-place>`

In order to add a real advertising content, the user should do the following:  

1. Open Marketing Module;
1. Select ‘Dynamic content’ tab;
1. Select ‘Content items’;
1. Select a specific content item from the list, to which the advertising content will be applied;
1. Insert the code into HTML text box and save the changes;
1. The new content will appear immediately on the web page.  

![Fig. Advertising Sport](docs/media/screen-advertising-spot.png)

## Promotions

A **Promotion** is a marketing tool used to increase sales. Promotions are store-specific and cannot be shared across multiple stores. To create a promotion, the user should have the 'Manage promotions' permission.

**Important**: In order to apply a promotion to multiple stores, the admin should manually re-create it for each applicable store.

[Promotion Rules](/docs/promotion-rules.md)

[Manage Promotions](/docs/manage-promotions.md)

[Combine Active Promotions](/docs/combine-active-promotions.md)

## Documentation

Developer guide: <a href="https://virtocommerce.com/docs/vc2devguide/working-with-platform-manager/extending-functionality/composing-dynamic-conditions" target="_blank">Composing dynamic conditions</a>

## Installation
Installing the module:
* Automatically: in VC Manager go to Configuration -> Modules -> Marketing module -> Install
* Manually: download module zip package from https://github.com/VirtoCommerce/vc-module-marketing/releases. In VC Manager go to Configuration -> Modules -> Advanced -> upload module package -> Install.

## Available resources
* Module related service implementations as a <a href="https://www.nuget.org/packages/VirtoCommerce.MarketingModule.Data" target="_blank">NuGet package</a>
* API client as a <a href="https://www.nuget.org/packages/VirtoCommerce.MarketingModule.Client" target="_blank">NuGet package</a>
* API client documentation http://demo.virtocommerce.com/admin/docs/ui/index#!/Marketing_module
