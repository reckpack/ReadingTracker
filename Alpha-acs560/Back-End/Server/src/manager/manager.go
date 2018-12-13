package manager

//import "category"
import (
	"category"
	"fmt"
	"mydatabase"
	"record"
	"strconv"
	"strings"
	"time"
)

var mCategories map[int]category.Category

// Init - Initializes the manager with whatever categories and records currently on the database
func Init() {
	mCategories = make(map[int]category.Category)
	cats := mydatabase.GetCategories()

	for _, c := range cats {
		catID := c.GetID()
		mCategories[catID] = c
		recs := mydatabase.GetAllRecords(catID)
		if len(recs) > 0 {
			for _, r := range recs {
				fmt.Printf("Record retrieved: id[%d] category id[%d] %s\n", r.GetRecordID(), r.GetCategoryID(), r.GetTitle())
				if r.GetIsFinished() == false {
					if record.CalculateAndUpdateNextReleaseDateAndIssue(&r) {
						//r.PrintNextReleaseDate()
						mydatabase.UpdateRecordReleaseDateAndLatestReleaseValue(r)
					}
				}
				mCategories[catID].InsertRecord(r)
				fmt.Println(r)
			}
		}
	}
}

// AddCategories - creates a new category and submits it to the database
//						if storage in DB is successful then it stores it locally
func AddCategories(_c string) int {

	id := mydatabase.AddCategory(_c)
	parameters := strings.Split(_c, ",")

	if id > 0 {
		trimmedIndex := strings.Trim(parameters[2], "'")
		indexInEditor, _ := strconv.Atoi(trimmedIndex)
		c := category.New(id, parameters[0], parameters[1], indexInEditor)
		mCategories[id] = c
		fmt.Printf("key[%d] value: "+c.GetName()+"\n", id)
		return id
	}
	return -1
}

//GetAllCategories - returns all the categories
func GetAllCategories() []category.Category {
	var cats []category.Category

	for k := range mCategories {
		cats = append(cats, mCategories[k])
	}
	return cats
}

// UpdateCategory - updates category that matches _id with the data passed in _action
func UpdateCategory(_id int, _action string) bool {

	if _id > 0 {
		parameters, result := mydatabase.UpdateCategory(_action, _id)
		if result {
			for k, c := range mCategories {
				if k == _id {
					if parameters[0] != "" { //} || parameters[0] != "''" {
						(&c).EditName(parameters[0])
					}
					// api link
					if parameters[1] != "" { //|| parameters[1] != "''" {
						(&c).EditAPILink(parameters[1])
					}
					// index in editor
					if parameters[2] != "" { //|| parameters[2] != "''" {
						trimmedIndex := strings.Trim(parameters[3], "'")
						index, _ := strconv.Atoi(trimmedIndex)
						(&c).EditIndexInEditor(index)
					}
					mCategories[c.ID] = c
					return true
				}
			}
		}
	}

	return false
}

// RemoveCategory - gets rid of category if it exists and records related to that category
func RemoveCategory(_id int) bool {
	if _id > 0 {
		if mydatabase.RemoveCategory(_id) {
			fmt.Printf("Category id: %d removed from database along with related records...\n", _id)
			fmt.Println("Removing local data of category and records")
			delete(mCategories, _id)
			return true
		}
		return false
	}
	return false
}

// GetAllRecords - gets all the records
func GetAllRecords(_catID int) []record.Record {
	var recsToReturn []record.Record
	CurRecs := *mCategories[_catID].GetAllRecords()
	for r := range CurRecs {
		recsToReturn = append(recsToReturn, CurRecs[r])
	}
	return recsToReturn
}

// GetCategoryIDs - gets the ids for the init function
func GetCategoryIDs() []int {
	var ids []int
	for key := range mCategories {
		ids = append(ids, key)
	}
	return ids
}

// AddRecord - adds record to DB
func AddRecord(_c string) int {
	id, catID, isfinished, parameters := mydatabase.AddRecord(_c)
	/*
	   string toGo = "'" + newRecord._Cat_pk + "', '" + newRecord.Name + "', '" + newRecord.CurrentRead +
	                       "', '" + newRecord.LatestRelease + "', '" + newRecord.NextRelease + "', '" + newRecord.Genre +
	                       "', '" + newRecord.Publisher + "', '" + newRecord.FirstRelease + "', '" + newRecord.Writer +
	                       "', '" + newRecord.Artist + "', '" + newRecord.IsFinished + "', '" + newRecord.ReleaseSchedule+
	                       "'";
	*/

	if id > 0 {
		nextReleaseDate := clientDateToGoDate(parameters[4])

		currReadStr := strings.Trim(parameters[2], "'")
		currentRead, _ := strconv.Atoi(currReadStr)

		latestReleaseStr := strings.Trim(parameters[3], "'")
		latestRelease, _ := strconv.Atoi(latestReleaseStr)

		initialRelease := clientDateToGoDate(parameters[7])
		r := record.New(id, catID, parameters[1], currentRead, latestRelease, nextReleaseDate,
			parameters[5], parameters[6], initialRelease, parameters[8], parameters[9], isfinished, parameters[11])

		mCategories[catID].InsertRecord(r)
		fmt.Printf("category[%d] value: %s inserted\n", catID, parameters[1])
		return id
	}
	return -1
}

// UpdateRecord - updates a record
func UpdateRecord(_id int, _action string) bool {

	if _id >= 0 {
		parameters, result := mydatabase.UpdateRecord(_id, _action)
		if result {
			for _, c := range mCategories {
				if c.IsRecordExist(_id) {
					r := c.GetRecord(_id)
					for i, s := range parameters {

						switch sw := i; sw {
						case 0:
							{
								r.UpdateName(s) //r.Updater = r.UpdateName
								break
							}
						case 1:
							{
								currReadStr := strings.Trim(s, "'")
								ci, _ := strconv.Atoi(currReadStr)
								r.UpdateCurrentIssue(ci) //r.Updater = r.UpdateName
								break
							}
						case 2:
							{
								currReadStr := strings.Trim(s, "'")
								lr, _ := strconv.Atoi(currReadStr)
								r.UpdateLatestReleased(lr) // r.Updater = r.UpdateName
								break
							}
						case 3:
							{
								r.UpdateGenre(s) //r.Updater = r.UpdateName
								break
							}
						case 4:
							{
								FinalDate := clientDateToGoDate(s)
								r.UpdateNextReleaseDate(FinalDate) //r.Updater = r.UpdateName
								break
							}
						case 5:
							{
								r.UpdateWriter(s) //r.Updater = r.UpdateName
								break
							}
						case 6:
							{
								r.UpdateArtist(s) //r.Updater = r.UpdateName
								break
							}
						case 7:
							{
								catID, _ := strconv.Atoi(s)
								r.UpdateCategoryID(catID) //r.Updater = r.UpdateName
								break
							}
						case 8:
							{
								r.UpdatePublisher(s) //r.Updater = r.UpdateName
								break
							}
						case 9:
							{

								date := clientDateToGoDate(s)    //record.Stod(s)
								r.UpdateInitialReleaseDate(date) //r.Updater = r.UpdateName
								break
							}
						case 10:
							{
								isFinished, _ := strconv.ParseBool(s)
								r.UpdateIsFinished(isFinished) //r.Updater = r.UpdateName
								break
							}
						case 11:
							{
								r.UpdateReleaseSchedule(s) //r.Updater = r.UpdateName
								break
							}
							//default:
							//	{
							//		updateCounter = 0
							//	}
						}
					}
					mCategories[c.ID].UpdateRecord(*r)
					fmt.Println(r)
					return true
				}
			}
		}
	}
	return false
}

func clientDateToGoDate(s string) time.Time {
	nosingleQuotes := strings.Replace(s, "'", "", -1)
	subDate := strings.Split(nosingleQuotes, " ") //record.Stod(s)
	dateComps := strings.Split(subDate[0], "/")
	year, _ := strconv.Atoi(dateComps[2])
	month, _ := strconv.Atoi(dateComps[0])
	day, _ := strconv.Atoi(dateComps[1])
	//dateobj := fmt.Sprintf("%s-%s-%s", year, month, day)
	FinalDate := time.Date(year, time.Month(month), day, 0, 0, 0, 0, time.UTC)
	return FinalDate
}

// RemoveRecord - removes the specified record from local storage and performs db call to do the same
func RemoveRecord(_id int) bool {

	if _id >= 0 {
		if mydatabase.RemoveRecord(_id) {
			// find which category this record is in
			for k := range mCategories {
				if mCategories[k].IsRecordExist(_id) {
					mCategories[k].RemoveRecord(_id) //delete(m.mCategories, _id)
					return true
				}
			}
		}
	}
	return false
}
