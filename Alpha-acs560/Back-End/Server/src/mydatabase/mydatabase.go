package mydatabase

import (
	"category"
	"database/sql"
	"fmt"
	"record"
	"strconv"
	"strings"
	"time"

	// this is what the tutorial said to do
	_ "github.com/lib/pq"
)

const (
	host     = "db.cx3km69qaogq.us-east-1.rds.amazonaws.com"
	port     = 5432
	user     = "master"
	password = "acs560db"
	dbname   = "acs560"
)

var mSQLDB *sql.DB
var psqlInfo string

// Init - starts the engines
func Init() *sql.DB {
	mSQLDB = nil
	// first lets connect to our database
	psqlInfo = fmt.Sprintf("host=%s port=%d user=%s "+
		"password=%s dbname=%s sslmode=disable", // sslmode=disable
		host, port, user, password, dbname)

	// connecting to our database, here we go captain
	var err error
	mSQLDB, err = sql.Open("postgres", psqlInfo)
	if err != nil {
		mSQLDB.Close()
		panic(err)
	}

	// test ping to make sure we made it
	err2 := mSQLDB.Ping()
	if err2 != nil {
		mSQLDB.Close()
		panic(err2)
	}
	fmt.Println("we pinged")
	if mSQLDB == nil {
		fmt.Println("ERROR: Failed to connerct to database")
		return nil
	}

	fmt.Println("Connection to database successful:\nConnection info: " + psqlInfo)

	return mSQLDB
}

// IsConnected - Tells the called if db connected
func IsConnected() bool {
	if mSQLDB != nil {
		return true
	}
	return false
}

// GetCategories - returns the rows with the retrieved records
func GetCategories() []category.Category {
	rows, err := mSQLDB.Query("SELECT * FROM CATEGORIES;")

	var cats []category.Category
	var id, indexInEditor int
	var name, apilink string

	for rows.Next() {

		err := rows.Scan(&id, &name, &apilink, &indexInEditor)
		if err == nil {
			c := category.New(id, name, apilink, indexInEditor)
			cats = append(cats, c)
			fmt.Printf("key[%d] value: "+c.GetName()+"\n", id)
		} else {
			fmt.Println(err)
			panic(err)
		}
	}

	if CheckErrorJustALittleBitAlarmed(err) {
		return nil
	}
	return cats
}

// AddCategory - adds a category from the client
func AddCategory(_command string) int {
	parameters := strings.Split(_command, ",")
	trimmedIndex := strings.Trim(parameters[2], "'")
	indexInEditor, _ := strconv.Atoi(trimmedIndex)
	sqlCommand := fmt.Sprintf("INSERT INTO public.categories(name,api_link, index_in_editor) VALUES (%s,%s,%d)", parameters[0], parameters[1], indexInEditor)
	_, err := mSQLDB.Exec(sqlCommand)
	if CheckErrorJustALittleBitAlarmed(err) {
		return -1
	}

	idSQL := mSQLDB.QueryRow("SELECT MAX(id) FROM CATEGORIES")
	var id int
	err2 := idSQL.Scan(&id)
	CheckErrorJustALittleBitAlarmed(err2)

	return id
}

// UpdateCategory - updates the specified category
func UpdateCategory(_command string, _id int) ([]string, bool) {
	parameters := strings.Split(_command, ",")
	compound := "SET "

	// name
	if parameters[0] != "" { //|| parameters[0] != "''" {
		compound += "NAME=" + parameters[0] + ", "
	}
	// api link
	if parameters[1] != "" { //|| parameters[1] != "''" {
		compound += "API_LINK=" + parameters[1] + ", "
	}
	// index in editor
	if parameters[2] != "" { //|| parameters[2] != "''" {
		compound += "INDEX_IN_EDITOR=" + parameters[2]
	}
	compound += " WHERE ID=" + strconv.Itoa(_id)

	sqlCommand := "UPDATE CATEGORIES " + compound
	res, err := mSQLDB.Exec(sqlCommand)
	if CheckErrorJustALittleBitAlarmed(err) {
		return nil, false
	}
	rowsAffected, _ := res.RowsAffected()

	if rowsAffected <= 0 {
		return nil, false
	}
	return parameters, true

}

// RemoveCategory - removes a category if the id exists. If it returns false, either we errored out or there was no record with that ID to remove
// 					if category is deleted then cascade delete all records related to that category
func RemoveCategory(_id int) bool {

	sqlCommand := fmt.Sprintf("DELETE FROM RECORDS WHERE category_id=%d", _id)
	result, _ := mSQLDB.Exec(sqlCommand)

	sqlCommand = fmt.Sprintf("DELETE FROM CATEGORIES WHERE id=%d", _id)
	result, err := mSQLDB.Exec(sqlCommand)
	if CheckErrorJustALittleBitAlarmed(err) {
		return false
	}

	rowsAffected, err2 := result.RowsAffected()
	if CheckErrorJustALittleBitAlarmed(err2) {
		return false
	}

	if rowsAffected <= 0 {
		return false
	}

	return true
}

// GetAllRecords - returns the records belonging to the specified category
func GetAllRecords(_index int) []record.Record {
	//fmt.Printf("key[%d] value: %s\n", k, v.GetName())
	sqlCommand := fmt.Sprintf("SELECT * FROM RECORDS WHERE category_id=%d", _index)
	rows, err := mSQLDB.Query(sqlCommand)
	if CheckErrorJustALittleBitAlarmed(err) {
		return nil
	}

	var recs []record.Record
	var ID int
	var CategoryID int
	var Title string
	var CurrentIssue int
	var LatestReleased int
	var NextReleaseDate time.Time
	var Genre string
	var Publisher string
	var InitialReleaseDate time.Time
	var Writter string
	var Artist string
	var IsFinished bool
	var ReleaseSchedule string

	for rows.Next() {
		//NextReleaseDateInTimeForm.
		err = rows.Scan(&Title, &CurrentIssue, &LatestReleased, &Genre, &NextReleaseDate,
			&Writter, &Artist, &CategoryID, &Publisher, &InitialReleaseDate, &ID, &IsFinished, &ReleaseSchedule)
		if err == nil {
			//c := category.New(id, name, apilink, indexInEditor)
			r := record.New(ID, CategoryID, Title, CurrentIssue, LatestReleased, NextReleaseDate, Genre, Publisher,
				InitialReleaseDate, Writter, Artist, IsFinished, ReleaseSchedule)

			recs = append(recs, r)
		}
	}

	return recs
}

// AddRecord - adds record to db
func AddRecord(_command string) (int, int, bool, []string) {
	parameters := strings.Split(_command, ",")
	/*
			 string toGo = "'" + newRecord._Cat_pk + "', '" + newRecord.Name + "', '" + newRecord.CurrentRead +
		                     "', '" + newRecord.LatestRelease + "', '" + newRecord.NextRelease + "', '" + newRecord.Genre +
		                     "', '" + newRecord.Publisher + "', '" + newRecord.FirstRelease + "', '" + newRecord.Writer +
		                     "', '" + newRecord.Artist + "', '" + newRecord.IsFinished + "', '" + newRecord.ReleaseSchedule+
		                     "'";
	*/
	// SO LONG!
	catID, _ := strconv.Atoi(parameters[0])
	isfinishedint, _ := strconv.Atoi(parameters[11])
	var isFinished bool
	if isfinishedint == 1 {
		isFinished = true
	} else if isfinishedint == 0 {
		isFinished = false
	}

	/*

		dateobj := actionDateStringToLocalDate(parameters[index])
			//FinalDate := time.Date(year, time.Month(month), day, 0, 0, 0, 0, time.UTC)
			compound += ",NEXT_RELEASE_DATE= " + fmt.Sprintf("to_date('%s','YYYY-MM-DD')", dateobj) //parameters[index]
	*/

	nrd := fmt.Sprintf("to_date('%s','YYYY-MM-DD')", actionDateStringToLocalDate(parameters[4]))
	ird := fmt.Sprintf("to_date('%s','YYYY-MM-DD')", actionDateStringToLocalDate(parameters[7]))

	// so string... much long...wow
	precommand := "INSERT INTO public.records(category_id,name,current_read, latest_released, next_release_date, genre, " +
		"publisher, initial_release_date, writter, artist, is_finished, release_schedule) VALUES (%d,%s,%s,%s,%s,%s,%s,%s,%s,%s,%t,%s)"

	sqlCommand := fmt.Sprintf(precommand, catID, parameters[1], parameters[2], parameters[3], nrd, parameters[5],
		parameters[6], ird, parameters[8], parameters[9], isFinished, parameters[11])
	_, err := mSQLDB.Exec(sqlCommand)
	CheckErrorJustALittleBitAlarmed(err)

	idFromDB := mSQLDB.QueryRow("SELECT MAX(id) FROM RECORDS")
	var id int
	err2 := idFromDB.Scan(&id)
	CheckErrorJustALittleBitAlarmed(err2)

	return id, catID, isFinished, parameters
}

// RemoveRecord - removes a record if the id exists. If it returns false, either we errored out or there was no record with that ID to remove
func RemoveRecord(_id int) bool {
	sqlCommand := fmt.Sprintf("DELETE FROM RECORDS WHERE id=%d", _id)
	result, err := mSQLDB.Exec(sqlCommand)
	if CheckErrorJustALittleBitAlarmed(err) {
		return false
	}

	rowsAffected, err2 := result.RowsAffected()
	if CheckErrorJustALittleBitAlarmed(err2) {
		return false
	}

	if rowsAffected <= 0 {
		return false
	}
	return true
}

func actionDateStringToLocalDate(_param string) string {
	nosingleQuotes := strings.Replace(_param, "'", "", -1)
	subDate := strings.Split(nosingleQuotes, " ") //record.Stod(s)
	dateNoSlash := strings.Replace(subDate[0], "/", "-", -1)
	dateComps := strings.Split(dateNoSlash, "-")
	year, _ := strconv.Atoi(dateComps[2])
	month, _ := strconv.Atoi(dateComps[0])
	day, _ := strconv.Atoi(dateComps[1])
	dateobj := fmt.Sprintf("%d-%d-%d", year, month, day)
	return dateobj
}

// UpdateRecord - updates record
func UpdateRecord(_id int, _command string) ([]string, bool) {

	parameters := strings.Split(_command, ",")
	compound := "SET "
	index := 0

	/*
	   [0]:"'Doctor Strange'"
	   [1]:"'4'"
	   [2]:"'9'"
	   [3]:"'action'"
	   [4]:"'2018-11-15'"
	   [5]:"'Mark Waid'"
	   [6]:"'Jesus Saiz'"
	   [7]:"'19'"
	   [8]:"'Marvel'"
	   [9]:"'2018-08-01'"
	   [10]:"'False'"
	   [11]:"''"
	*/

	// name
	if parameters[0] != "" { //|| parameters[0] != "''" {
		compound += "NAME=" + parameters[0]
	}
	index++
	// current read
	if parameters[index] != "" { //|| parameters[1] != "''" {
		compound += ",CURRENT_READ=" + parameters[index]
	}
	index++
	// latest read
	if parameters[index] != "" { //|| parameters[1] != "''" {
		compound += ",LATEST_RELEASED=" + parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		compound += ",GENRE=" + parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		/*nosingleQuotes := strings.Replace(parameters[index], "'", "", -1)
		subDate := strings.Split(nosingleQuotes, " ") //record.Stod(s)
		dateNoSlash := strings.Replace(subDate[0], "/", "-", -1)
		dateComps := strings.Split(dateNoSlash, "-")
		year, _ := strconv.Atoi(dateComps[2])
		month, _ := strconv.Atoi(dateComps[1])
		day, _ := strconv.Atoi(dateComps[0])
		dateobj := fmt.Sprintf("%d-%d-%d", year, month, day)*/
		dateobj := actionDateStringToLocalDate(parameters[index])
		//FinalDate := time.Date(year, time.Month(month), day, 0, 0, 0, 0, time.UTC)
		compound += ",NEXT_RELEASE_DATE= " + fmt.Sprintf("to_date('%s','YYYY-MM-DD')", dateobj) //parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		compound += ",WRITTER=" + parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		compound += ",ARTIST=" + parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		compound += ",CATEGORY_ID=" + parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		compound += ",PUBLISHER=" + parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		/*subDate := strings.Split(parameters[index], " ") //record.Stod(s)
		dateComps := strings.Split(subDate[0], "//")
		year, _ := strconv.Atoi(dateComps[2])
		month, _ := strconv.Atoi(dateComps[1])
		day, _ := strconv.Atoi(dateComps[0])
		dateobj := fmt.Sprintf("%d-%d-%d", year, month, day)*/
		dateobj := actionDateStringToLocalDate(parameters[index])
		compound += ",INITIAL_RELEASE_DATE=" + fmt.Sprintf("to_date('%s','YYYY-MM-DD')", dateobj) //parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		compound += ",IS_FINISHED=" + parameters[index]
	}
	index++
	// index in editor
	if parameters[index] != "" { //|| parameters[2] != "''" {
		compound += ",RELEASE_SCHEDULE=" + parameters[index]
	}

	compound += " WHERE ID=" + strconv.Itoa(_id)
	sqlCommand := "UPDATE RECORDS " + compound
	fmt.Println(sqlCommand)
	res, err := mSQLDB.Exec(sqlCommand)
	if CheckErrorJustALittleBitAlarmed(err) {
		return nil, false
	}
	rowsAffected, _ := res.RowsAffected()

	if rowsAffected <= 0 {
		return nil, false
	}

	return parameters, true
}

// UpdateRecordReleaseDateAndLatestReleaseValue - does what it says it does
func UpdateRecordReleaseDateAndLatestReleaseValue(r record.Record) bool {

	//r.GetLatestReleased(), r.GetNextReleaseDate(), r.GetRecordID()
	_rd := r.GetNextReleaseDate()
	_lr := r.GetLatestReleased()
	_id := r.GetRecordID()
	y, m, d := _rd.Date()
	releasedatestring := fmt.Sprintf("'%d-%d-%d'", y, m, d)
	//fmt.Println(releasedatestring)
	//releasedatestring := fmt.Sprintf("'%s'", _rd.String())
	sqlCommand := fmt.Sprintf("UPDATE RECORDS SET NEXT_RELEASE_DATE=%s, LATEST_RELEASED=%d WHERE ID=%d", releasedatestring, _lr, _id)
	fmt.Println(sqlCommand)
	res, err := mSQLDB.Exec(sqlCommand)
	if CheckErrorJustALittleBitAlarmed(err) {
		return false
	}
	rowsAffected, _ := res.RowsAffected()

	if rowsAffected <= 0 {
		return false
	}
	return true
}

// CloseDatabase - bye bye
func CloseDatabase() {
	if mSQLDB != nil {
		err := mSQLDB.Close()
		CheckErrorPanic(err)
	}
}

// CheckErrorPanic - should we panic?
func CheckErrorPanic(err error) {
	if err != nil {
		panic(err)
	}
}

// CheckErrorJustALittleBitAlarmed - returns true if there was an error (but not bad enough that panic is necessary). returns false if there was no error
func CheckErrorJustALittleBitAlarmed(err error) bool {
	if err != nil {
		fmt.Println(err.Error())
		return true
	}
	return false
}
