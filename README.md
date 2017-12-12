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


Recommendation algorithm specifics:
1. Take all beers selected by users
2. Group tags they contain and count occurrencess for each tag (which become the weight for the tag)
3. From all beers take only the ones that contain at least one of the grouped tags except the user selected beers
4. Assign score to each beer based on sum of the weights for each tag they contain
5. Pick the top 5 beers

We also tried enhancement for the algorithm (but since we did not manage to test it extensively we did not include it):
* In step 5 select only top 3 beers, then select 2 more beers randomly from the top 4.-10. positions.
