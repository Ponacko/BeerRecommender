## Beer Recommender
Project Structure:
- **BL**: Bussiness Layer. Contains Service methods that use underlaying DAL. Most importantly contains Recommend method (in RecommendationService) in which main recommendation algorithm is present.

- **BeerRecommender**: DAL Layer. Contains entities, DB migrations and repositories for working with the data in the DB. Also contains scraper for parsing the data from the source web and saving it to the DB.

- **BussinessLayer**: Redundant BL, will be removed.

- **Web**: UI, should be used as startup project. Runs BeerRecommender website, uses Service methods.


Work distribution:
- Tomas P. (Data scraping and parsing, Recommendation algorithm, Beer picking for the frontpage, some DB migrations)
- Matus M. (Entities, service methods, presentation, some DB migrations)
- Tomas S. (Project set-up, DB initialization, Web UI, some DB migrations, parsing set-up)
