//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
// Version: 1.0.0                                   //
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

  // Get url for index page
  let url = window.location.origin + "/index.html";

  // Request html content from the index page
  const htmlReq = new XMLHttpRequest();
  htmlReq.open("GET", url);
  htmlReq.addEventListener("load", () => {
    // Open document
    document.open();

    // Proceed if the new document is loaded
    document.addEventListener("DOMContentLoaded", () => {
      // Check if a value for nav to is provided
      if (navToValue != null) {
        // Wrap the original navTo function from doxygen into custom function
        let originalNavToFunc = navTo;
        navTo = (o, root, hash, relpath) => {
          // Ignore the root value and parse or nav to value instead
          originalNavToFunc(o, navToValue, hash, relpath);
        };
      }

      // Get the div that contains the content
      let containerChildren = document.getElementById("doc-content").children;
      let contentDiv;
      for (let i = 0; i < containerChildren.length; i++) {
        if (
          containerChildren[i].classList.length <= 0 &&
          containerChildren[i].id.trim() == ""
        ) {
          // The content div has no classes or id
          contentDiv = containerChildren[i];
        }
      }

      // Exit if no content div was found and display error message
      if (contentDiv == null) {
        document.body.innerHTML = "";
        document.body.innerText = "Failed to load custom page";
        return;
      }

      // Replace document content
      contentDiv.innerHTML = injectionWrapper.innerHTML;
    });

    // Replace and close document
    document.write(htmlReq.responseText);
    document.close();
  });

  // Sent request
  htmlReq.send();
});
