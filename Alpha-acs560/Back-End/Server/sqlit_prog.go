package main

import (
	"database/sql"
	"fmt"

	_ "github.com/mattn/go-sqlite3"
)

func main() {
	db, err := sql.Open("sqlite3", "./p4.db")
	checkErr(err)

	// create table
	statement, _ := db.Prepare("CREATE TABLE IF NOT EXISTS courses (id INTEGER PRIMARY KEY, semester TEXT, course TEXT)")
	statement.Exec()

	insertRecord(db, "Fall 2018", "Software Engineering")
	insertRecord(db, "Spring 2018", "Software Project Management")
	// query

	printRecords(db)
	db.Close()

}

func printRecords(db *sql.DB) {
	rows, err := db.Query("SELECT * FROM courses")
	checkErr(err)
	var uid int
	var semester string
	var course string

	fmt.Println("Semester\t\tCourse")
	for rows.Next() {
		err = rows.Scan(&uid, &semester, &course)
		checkErr(err)
		fmt.Print(semester + "\t\t" + course + "\n")
	}

	rows.Close() //good habit to close
}

func insertRecord(db *sql.DB, semester string, course string) {
	statement := fmt.Sprintf("INSERT INTO courses(semester, course) values('%s','%s')", semester, course)
	fmt.Println(statement)
	// insert
	stmt, err := db.Prepare(statement)
	checkErr(err)

	res, err2 := stmt.Exec()
	checkErr(err2)
	_, err3 := res.LastInsertId()
	checkErr(err3)

}

func checkErr(err error) {
	if err != nil {
		panic(err)
	}
}
