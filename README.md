Unit tests are created to test the Orders service for the requirements in the Assignment, which are 
- Return a list with all orders
- Return a specific order based on a unique identifier
- Create an order
- Update a field on an existing order
- Delete all orders

The solution uses in memory Sqlite database, but configured it to use Sql Server in non Development environments. 

It has cache service just to show possible implementation for speeding up response (but in this case 
there isn't any benefit since the database itself is in memory.)


I assumed the issue of Users, Authentication and Authorization, Product Catalogue, Pricing etc is not part of the assignment
and the data coming is reliable in that sense.  Thus validation is kept to minimum.    