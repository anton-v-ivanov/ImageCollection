#Simple rest api file storage

How to start from Visual Studio:
1. Solution properties -> Startup project -> Choose "Start" for ImageCollections.Service + ImageCollections.WebApi
2. Start solution

Default port is 8959.

##Use case example

##Images

###Upload an image 
`POST http://localhost:8959/api/image`
Request should be multipart/form-data with form parameter name "file" for file

###Download the image by a link in the JSON
`GET http://localhost:8959/api/image/{id}`

###Get image information (metadata)
`GET http://localhost:8959/api/image/{id}/info`

###Get list of images with metadata
`GET http://localhost:8959/api/images`

###Update image name
`PATCH http://localhost:8959/api/images/{id}/info`
```
{
	"name": "Image name"
}
```

###Delete image
`DELETE http://localhost:8959/api/images/{id}`
Note: Image will be removed from all collections in case of deletion.

##Collections

###Create a collection
`POST http://localhost:8959/api/collection`
```
{
  "name" : "Collection name"
}
```

###Get list of collections
`GET http://localhost:8959/api/collections`

###Get list of images by a collection
`GET http://localhost:8959/api/collection/{id}` OR `GET http://localhost:8959/api/collection/{id}/images`

###Add an image to the collection
`POST http://localhost:8959/api/collection/{id}/images`
```
{
  "imageId":{imageId}
}
```

###Remove image from the collection
`DELETE http://localhost:8959/api/collection/{collectionId}/images/{imageId}`

###Update name of collection
`PATCH http://localhost:8959/api/collection/{id}`
```
{
  "name":"New collection name"
}
```

###Delete collection
`DELETE http://localhost:8959/api/collection/{Id}`