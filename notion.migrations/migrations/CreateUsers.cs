using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace notion.migrations.migrations
{
    [Migration(1)]
    public class CreateUsers : Migration
    {
        public override void Down()
        {
            Delete.Table("users");
        }

        public override void Up()
        {
            Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey().Identity()
            .WithColumn("email").AsString().NotNullable().Unique()
            .WithColumn("creation_date").AsDateTime().WithDefaultValue(RawSql.Insert("now()"))
            .WithColumn("modification_date").AsDateTime().Nullable()
            ;

        }
    }
}