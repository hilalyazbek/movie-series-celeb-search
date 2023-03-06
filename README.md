# credit-suisse

### Docker
https://hub.docker.com/r/hyazbek/creditsuisse

#### Technology used
dotnet core 7.0
EntityFrameworkCore
postgres
docker
JWT
AutoMapper

#### Azure
The services are deployed to Azure on:
-       https://csuserdetails.azurewebsites.net/auth/login

        {
        "email": "admin@test.com",
        "password": "P@ssw0rd"
        }

-       Send Token with every request
-       GetUsers: https://csuserdetails.azurewebsites.net/users?PageNumber=1&PageSize=100&SortBy=username
-       GetUserDetails: https://csuserdetails.azurewebsites.net/users/667ed7d9-8ba8-40bd-8323-476451910f04
-       SearchMovies: https://csmovies.azurewebsites.net/movies?searchQuery=RRR
-       SearchSeries: https://csmovies.azurewebsites.net/series?searchQuery=friends&PageNumber=1
-       SearchCelebrities: https://csmovies.azurewebsites.net/celebrities?searchQuery=tom hardy
-       AddToWatchList: https://csmovies.azurewebsites.net/watchlater
        
        {
        "userId": "ed58c587-d934-4037-8954-563a5a491376",
        "programId": 34453,
        "programName": "Interstellar"
        }
-       GetWatchList: https://csmovies.azurewebsites.net/watchlater/ed58c587-d934-4037-8954-563a5a491376
-       DeleteFromWatchList: https://csmovies.azurewebsites.net/watchlater
        {
        "userId": "ed58c587-d934-4037-8954-563a5a491376",
        "programId": 543
        }
-       RateMovie: https://csmovies.azurewebsites.net/ratings
        {
        "programId": 1232,
        "ratingValue": 3.5
        }
-       GetAllRatings: https://csmovies.azurewebsites.net/ratings
-       GetRatingByProgramId: https://csmovies.azurewebsites.net/ratings/1232
-       GetSearchHistory: https://csmovies.azurewebsites.net/searchhistory?PageNumber=1&PageSize=100&SortBy=query

##### Download the Postman Collection from the Repo
creditsuisse.postman_collection.json


#### Logging
-       http://[hostname]/watchdog
-       username: admin
-       password: admin

#### Run the Solution
Pull the latest Postgres Image, run the below command
```
docker run --name PostgresDb -p 5432:5432 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=P@ssw0rd -e POSTGRES_DB=user-details-db -d postgres
```

#### Deploy Database migration
cd to the application-infrastructure and run the below command to deploy the Database model into Postgres Image
```
dotnet ef database update
```

#### 1. Solution Architecture
the source code can be found in src folder, the unit tests are in tests folder.

##### 1.1   application-infrastructure
this project contains infrastructure code like Repositories, Logging, DBContext, Migrations, Token Service, Entities, Paging and Sorting.

The details of every function can be found inside the .cs files (i.e.)
```
/// <summary>
/// It creates a new search history record and returns all search history records
/// </summary>
/// <param name="SearchHistory">This is the model that I'm using to save the data.</param>
/// <returns>
/// IEnumerable<SearchHistory>
/// </returns>
public IEnumerable<SearchHistory> Save(SearchHistory search)
```
#### 1.1    user-details-service
this project contains the controllers, DTOs, and Mapping profiles

1.1.1   AuthController:
-   Register(CreateUserDTO request)
-   Authenticate([FromBody] AuthRequestDTO request)

1.1.2   UsersController: 
-   GetUsers([FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
-   UserDetails(string id)
-   DeleteUser(string id)
-   UpdateUser(string id, UpdateUserDTO user)

The details of every function can be found inside the .cs files (i.e.)

#### 1.2    movie-service
this project contains the controllers, DTOs, and Mapping profiles

1.2.1   SearchController
-   SearchMovies([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,[FromQuery]  sortingParameters)

-    SearchSeries([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters, [FromQuery] SortingParameters sortingParameters)

-   SearchCelebrities([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters, [FromQuery] SortingParameters sortingParameters)

#### 1.2.1   UserPreferencesController
-   GetWatchList(string userId)
-   AddToWatchLater([FromBody] CreateWatchLaterDTO request)
-   DeleteFromWatchLater([FromBody] DeleteWatchLaterDTO request)
-   RateProgram([FromBody] UpdateRatingDTO request)
-   GetRatings()
-   GetRatings(int programId)
-   GetSearchHistory([FromQuery] PagingParameters pagingParameters, [FromQuery] SortingParameters sortingParameters)
