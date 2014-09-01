bijectiv
========

*A bijection is a function between the elements of two sets, where every element of one set is paired with exactly one element of the other set, and every element of the other set is paired with exactly one element of the first set.* -- [wikipedia](http://en.wikipedia.org/wiki/Bijection)

*And* **bijectiv** *is an addictive object-to-object injection library for .NET.*

The aims of the **bijectiv** are to:

  * Provide awesome injection capabilities for objects and collections.
  * Expose a lean and clear API.
  * Be rigorously tested.
  * Be brilliantly documented.

(kind of ambitious, but things are definitely going in the right direction!) 
 
This project is currently under active development (see below for the latest update).

# Update 2014/09/01
# Update 2014/09/01

The initial release is expected towards the end of the year.

Currently the library is ~85% written. The unit test code coverage oscillates between 90% and 100%, but there is no integration test suite. A library like this is quite interconnected and has quite large foundations, so it is hard to write sensible integration tests until most of those are in place. Throwaway spike tests have been written in lieu of proper integration tests to assert that components function as expected. Once the library is functionally complete for the first release (1.31) the integration tests will get built out.

Current level of documentation is 0%. That will get written alongside the integration tests once the API is finalised.

## Features so far
  * Transform (create a new object of another type from an existing object).
  * Merge (merge an existing object into another existing object).
  * Collection transform and merge on heterogeneous collections (via an equivalence relation)
  * Automatically identify source and target members through some naming convention.
  * Member overrides to identify source and target explicitly.
  * Custom object factories.
  * Full support for any IoC container.
  * Custom actions that can be executed at various points in the transform / merge operations.
  * Inheritance of transform / merge rules with full support for polymorphic and non-polymorphic members.
 
## Non-functional
  * The architecture is based on a: build instructions, parse instructions, compile paradigm. This makes it very easy to build new features and modify existing ones.
  * The injection operations are all built using expression trees and then compiled at runtime, so in theory should perform about as well as handwritten code.