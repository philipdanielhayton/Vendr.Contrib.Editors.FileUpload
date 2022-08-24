# Introduction

This is an example of how to add a file upload editor to the order edit screen in Vendr. All the code required exists in the repo but you'll need to copy it over to your solution manually (I haven't had chance to turn it into a package yet).

- It works with multiple files
- It supports deletions
- It saves a simple json array to the properties model.value with the following model:

```
[
  {
    "name": "my file name.jpg",
    "url": "https://mydomain.com/path-to-my-file.jpg"
  }
]
```

- Files are saved to the server on submit, but the json data is saved when the order is saved.
- It's very much a rough draft, use it to get started and then polish as needed

## Installation

1. Copy the files in App_Plugins to your website project
2. Copy all other files to an appropriate lib project
3. Add the following key to the root of your app settings:

```
  "VendrFileUpload": {
    "RootFolder":  "orders"
  },
```

4. Set the `view` property in your Vendr order config `/App_Plugins/Vendr/config/{storeAlias}.order.editor.config.js`. For example:

```
    additionalInfo: [
        { alias: "myUploadFile", label: "My Upload File", view: "/App_Plugins/Vendr.Contrib.Editors.FileUpload/views/file-upload.html" }
    ],
```

### Things to note

##### Where are files saved to?

Out of the box files will be saved in your Umbraco website at `/{rootFolder}/{storeId}/{orderId}/{alias}`, where `rootFolder` is the value set in app settings.

##### Order state is not updated when files are uploaded

When files are uploded they are immediately persisted to disk and file meta data is sent back to the editor, however the editor must remember to save the order otherwise the meta data will not be saved to the order.
