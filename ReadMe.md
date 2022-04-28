## Qira Task

* All requested functional requirments implmenteded.
* Ef Core db also implemented - you can use it "AS IS", no need to run migration (InMemory DB is injected by DI).
* Swagger enabled for testing.
* To Switch to EF Core DB , add this enviorment variable:
```
DB_TYPE=EF
```

* I used repository pattern for data access.
* In the 2 implementaion of `IRepo` - I made generic classes, to enable use it for other models if needed.
* CSV is the default DB, so if you don't specify DB_TYPE, it will use CSV.
* Regading `GenericRepo` - I used sync version of EF methods, because it's only for testing. In real world app use case - async version (and Task...) will be preferred.
* To use the sort and filter parameters :
  * `sort` string example: "Id desc" - means sort by Id descending.
  * `filterColumn` + `filterTerm` - used to filter, for example: `filterColumn`='Amount' and `filterTerm`='100' - means filter by Amount=100.
* Serval more comments are in the code.

