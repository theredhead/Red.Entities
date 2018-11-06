# Red.Entities 

This is the beginnings of the backend library that will allow me to bounce Entities (database records) 
back and forth between clients and server while tracking changes and updating clients in realtime.

The backend server will keep track of all entities currently in use on any client and update all 
clients that have an entity in their scope the moment it's modifications are persisted.

This ties in with my entity database designer plans as currently functionally implemented in a rough 
draft within the separate TypedUI project. The designer currently only creates an init script for the
database but that will be expanded to create Entity implementations as well and eventually should be
able to generate an entire tiered web application from an entity design. That should alow getting an
application up and running almost instantaneous and allow for one-off code generation to aid in rapid
application development for complicated trees of data.

querying API looks like this:

    EntitiesDatabase db = Global.Current.Database;

    var request = db.Articles.CreateFetchRequest();
    var predicate = request.CreatePredicate();

    predicate
        .WhereStringFieldContains("Code", "ABC")
        .WhereDecimalFieldBetween("UnitPrice", 1.0M, 100.0M);

    var entities = db.Fetch(request);

    repeater.DataSource = entities;
    repeater.DataBind();

Parameterized SQL is generated behind the scenes to make the above code produce these queries:

When using MySqlEntityStatementBuilder:
````
	SELECT `Articles`.* 
	FROM `Articles` 
	WHERE 
		INSTR(`Code`, @Code) > 0 
	AND	`UnitPrice` BETWEEN @UnitPrice AND @UnitPrice1
````
When using SqlServerEntityStatementBuilder:
````
	SELECT [Articles].* 
	FROM [Articles] 
	WHERE 
		INSTR([Code], @Code) > 0 
	AND	[UnitPrice] BETWEEN @UnitPrice AND @UnitPrice1
````

