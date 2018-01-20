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


Weighted average algorithm specifics:
1. Take all beers selected by users
2. Group tags they contain and count occurrencess for each tag (which become the weight for the tag)
3. From all beers take only the ones that contain at least one of the grouped tags except the user selected beers
4. Assign score to each beer based on sum of the weights for each tag they contain
5. Pick the top 5 beers

We also tried enhancement for the algorithm (but since we did not manage to test it extensively we did not include it):
* In step 5 select only top 3 beers, then select 2 more beers randomly from the top 4.-10. positions.

Single similar algorithm:
1. Take all beers (filter by region if selected)
2. For each selected popular beer:
3. Take all beers that have at least one same tag as current popular beer
4. Order them by the amount of similar tags
5. Add the most similar one to the recommendation list

Close neighbours algorithm:
1. Take tags from picked beers
2. For each tuple <Beer, it’s Tags> tries to find largest tag intersect between other tuples (closest neighbour)
3. If large intersect is found (>3) save it as group, otherwise create group with 1 member only
4. For each group:
5. Take all beers that contain at least 1 tag from the group (filter for region if needed)
6. For each beer: Calculate weight (sum weights of all tags in group)
7. Add beer with largest weight as recommendation

Combined algorithm (Distinct off):
1. Calculate Weighted Average recommendations for picked beers, but keep weights
2. Calculate Single similar for each picked beer (but take more than just 1 beer recommendation for each picked beer) but keep weights
3. Normalize the weights of both groups
4. Combine both recommendations (Assign higher weights to the beers that were picked by both algorithms)
5. Order by highest coefficient and recommend top beers

Combined algorithm (Distinct on):
Steps 1 - 4 same as with distinct turned off
5. Then: Filter only beers with distinct weights (e. g. if 3 beers have weight 0.5574482 take only first one and ignore the rest with this weight)
6. Order by weights, descending
7. Recommend top n beers


