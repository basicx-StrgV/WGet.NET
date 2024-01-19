//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
// Version: 1.0.4                                   //
//--------------------------------------------------//
document.addEventListener("DOMContentLoaded", () => {
  // Get the injection wrapper element and nav to value
  let injectionWrapper = document.getElementById("injection-wrapper");
  let navToValue = injectionWrapper.getAttribute("nav-to");

  // Exit if there is no injection wrapper element and display error message
  if (injectionWrapper == null) {
    document.body.innerHTML = "";
    document.body.innerText = "Failed to load custom page";
    return;
  }

  // Get current url
  let currentLocation = String(window.location);
  // Find the last url separator
  let lastUrlSeperator = 0;
  for (let i = 0; i < currentLocation.length; i++) {
    // Save index of the current url separator
    if (currentLocation[i] == "/") {
      lastUrlSeperator = i;
    }
  }
  // Extract root url
  let rootUrl = currentLocation.slice(0, lastUrlSeperator);
  // Set index.html url
  let url = rootUrl + "/index.html";

  // Request html content from the index page
  const htmlReq = new XMLHttpRequest();
  htmlReq.open("GET", url);
  htmlReq.addEventListener("load", () => {
    // Open document
    document.open();

    // Proceed if the new document is loaded
    document.addEventListener("DOMContentLoaded", () => {
      try {
        if (navToValue != null && navToValue.trim() != "") {
          // Get doxygen nav content
          let navContent = document.getElementById("nav-tree-contents");

          if (navContent == null) {
            // Log error if content could not be found
            console.error(
              "[doxygen-custom-page-injector.js] The navigation can not be set to the current page, because the doxygen nav content could not be located"
            );
          } else {
            // Add event to check the nav content to remove the default nav and initiate the correct nav,
            // after the default nav was loaded
            let customNavInserted = false;
            navContent.addEventListener("DOMNodeInserted", (event) => {
              if (
                customNavInserted == false &&
                event.relatedNode == navContent
              ) {
                // Clean up of the nac default content
                for (let i = 0; i < navContent.children.length; i++) {
                  // Remove elements from the nav content, that are initalized with the nav
                  if (navContent.children[i].id != "nav-sync") {
                    navContent.removeChild(navContent.children[i]);
                  }
                }

                // Set the flag that indicates that the custom nav was inserted
                customNavInserted = true;

                // Initiate custom nav tree
                // (The "initNavTree(...)" function comes from the folowing doxygen script: "navtree.js")
                initNavTree(navToValue, "");
              }
            });
          }
        } else {
          console.error(
            "[doxygen-custom-page-injector.js] The navigation can not be set to the current page, bacause no 'nav-to' value provided."
          );
          return;
        }
      } catch (e) {
        console.log(
          "[doxygen-custom-page-injector.js] An error occured while trying to set the doxygeen nav:",
          e
        );
      }

      // Get the div that contains the content
      let contentContainer = document.getElementById("doc-content");

      // Exit if no content div was found and display error message
      if (contentContainer == null) {
        document.body.innerHTML = "";
        document.body.innerText = "Failed to load custom page.";
        console.error(
          "[doxygen-custom-page-injector.js] Failed to inject custom html content, because the doxygen content container could not be found."
        );
        return;
      }

      // Clear the content div
      for (let i = 0; i < contentContainer.children.length; i++) {
        if (
          contentContainer.children[i].id.trim() != "MSearchSelectWindow" &&
          contentContainer.children[i].id.trim() != "MSearchResultsWindow"
        ) {
          // Remove element from content if it does not belong to the layout
          contentContainer.removeChild(contentContainer.children[i]);
        }
      }

      // Create a content wrapper element and add the custom content
      let contentWrapper = document.createElement("div");
      contentWrapper.innerHTML = injectionWrapper.innerHTML;

      // Appand the content wrapper to the content container
      contentContainer.appendChild(contentWrapper);
    });

    // Replace and close document
    document.write(htmlReq.responseText);
    document.close();
  });

  // Sent request
  htmlReq.send();
});
