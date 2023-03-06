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
