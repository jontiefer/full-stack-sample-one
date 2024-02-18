#!/bin/bash
cd Developer.API
dotnet ef migrations add InitialIdentityMigration -c SampleAppDbContext
dotnet ef database update -c SampleAppDbContext
