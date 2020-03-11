# Jokes-from-icanhazdadjoke.com
Basic UI which you can get a random joke or search for jokes with one or many keywords. 
It highlights the words found and brings back up to 30 jokes categorized by 
Short (&lt;10 words), Medium (&lt;20 words), Long (>= 20 words). 

The UI pulls from a web API service in the same project which in turn consumes from the icanhazdadjoke.com web API.

The UI URL is:

/jokes

The web APIs are:

Get a random joke formatted as HTML

/jokesapi/get-random-return-html

Get a random joke formatted as JSON

/jokesapi/get-random-return-json

Search for jokes by keyword and return HTML. 
Case insensitive. Multiple words can be used if seperated by at least one space. Max of 30 results.
Brings back an HTML bulleted list categorized into groups based on the word sizes in the jokes.
The default groups are; Short (<10 words), Medium (<20 words), Long (>= 20 words).
The words found in the text are highlighted yellow.
These can be changed by passing in your own FilterCategories.

/jokesapi/search-return-html?term=dad
/jokesapi/search-return-html?term=happy dad

Search for jokes by keyword and return JSON
Case insensitive. Multiple words can be used if seperated by at least one space. Max of 30 results.

/jokesapi/search-return-json?term=dad
/jokesapi/search-return-json?term=happy dad



