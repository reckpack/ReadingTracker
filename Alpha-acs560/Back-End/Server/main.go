package main

import (
	"database/sql"
	"manager"
	"mydatabase"
	"network"

	// this is what the tutorial said to do
	_ "github.com/lib/pq"
)

// useful go sql stuff:
// https://astaxie.gitbooks.io/build-web-application-with-golang/en/05.4.html
// https://www.calhoun.io/updating-and-deleting-postgresql-records-using-gos-sql-package/

const (
	host     = "db.cx3km69qaogq.us-east-1.rds.amazonaws.com"
	port     = 5432
	user     = "master"
	password = "acs560db"
	dbname   = "acs560"
)

//var man manager.Manager

var mySQLDB *sql.DB
var psqlInfo string

// type sqlCommands struct {
// 	mRetrieveCategories string
// }

// func New() sqlCommands {
// 	var cm sqlCommands
// 	cm.mRetrieveCategories = "SELECT * FROM CATEGORIES"
// 	return m
// }

func main() {
	//rec := record.New("title", "genre", "next date", "current issue",
	//	"latest Released", "category", "publisher")
	network.LaunchServer()
	mydatabase.Init()
	if mydatabase.IsConnected() {
		manager.Init()
		loop := true
		for loop {
			loop = run()
		}
	}
	defer closeDownShop()

}

func run() bool {
	network.Listen()
	/* reader := bufio.NewReader(os.Stdin)
	fmt.Print("Press any key to exit: ")
	var text string
	text = ""
	text, _ = reader.ReadString('\n')
	if text != "" {
		return false
	} */

	return true
}

func deleteRecord(_id int) {
	if _id >= 0 {
		manager.RemoveRecord(_id)
	}
}

//closeDownShop - delete,closes,finishes,etc whatever the server is running
func closeDownShop() {
	mydatabase.CloseDatabase()
	network.CloseServer()
}
