//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
// Version: 1.0.2                                   //
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
      // Check if a value for nav to is provided
      if (navToValue != null && navTo != null) {
        // Wrap the original navTo function from doxygen into custom function
        let originalNavToFunc = navTo;
        navTo = (o, root, hash, relpath) => {
          // Ignore the root value and parse or nav to value instead
          originalNavToFunc(o, navToValue, hash, relpath);
        };
      }

      // Get the div that contains the content
      let contentContainer = document.getElementById("doc-content");

      // Exit if no content div was found and display error message
      if (contentContainer == null) {
        document.body.innerHTML = "";
        document.body.innerText = "Failed to load custom page";
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
