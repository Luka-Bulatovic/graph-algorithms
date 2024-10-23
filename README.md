# GraphsAlgorithms

In order to add new display Property to application:
- Add it to GraphPropertyEnum in Shared
- Add property and mapping for it in GraphProperties.cs in Core
- Add it to SeedGraphProperties method in ApplicationDBContext
- Add support for its data type if it doesn't exist (currently need to look for todos)
- Create migration for it
- Add logic for calculating it in GraphEvaluator