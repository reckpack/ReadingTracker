package record

import (
	"fmt"
	"strconv"
	"strings"
	"time"
)

// Record - record
type Record struct {
	MID                int       `json:"id"` // TODO: I dont understand Go well enough so for now the ID will be public but SHOULD NEVER EVER be changed BY ANYONE
	CategoryID         int       `json:"category_id"`
	Name               string    `json:"title"`
	CurrentIssue       int       `json:"current_issue"`
	LatestReleased     int       `json:"latest_released"`
	NextReleaseDate    time.Time `json:"next_release_date"`
	Genre              string    `json:"genre"`
	Publisher          string    `json:"publisher"`
	InitialReleaseDate time.Time `json:"initial_release_date"`
	Writer             string    `json:"writer"`
	Artist             string    `json:"artist"`
	IsFinished         bool      `json:"is_finished"`
	ReleaseSchedule    string    `json:"release_schedule"`
}

// New - Creates new record
func New(_id int, _catID int, _title string, _currentIssue int, _latestReleased int, _nextRelease time.Time, _genre string, _publisher string, _initialRelease time.Time,
	_writter string, _artist string, _isFinished bool, _releaseSchedule string) Record {
	r := Record{_id, _catID, _title, _currentIssue, _latestReleased, _nextRelease, _genre, _publisher, _initialRelease, _writter, _artist, _isFinished, _releaseSchedule} //, nil}
	return r
}

// PrintNextReleaseDate - dopes what it says it does
func (r Record) PrintNextReleaseDate() {
	fmt.Printf("%s next release date: %s\n", r.Name, r.NextReleaseDate.String())
}

// GetTitle - get title
func (r *Record) GetTitle() string {
	return r.Name
}

// GetRecordID - returns the record
func (r *Record) GetRecordID() int {
	return r.MID
}

// GetCategoryID - returns the record
func (r *Record) GetCategoryID() int {
	return r.CategoryID
}

// GetInitialReleaseDate - get initial release date
func (r *Record) GetInitialReleaseDate() time.Time {
	return r.InitialReleaseDate
}

// GetLatestReleased - does what it says
func (r *Record) GetLatestReleased() int {
	return r.LatestReleased
}

// GetNextReleaseDate - get latest release date
func (r *Record) GetNextReleaseDate() time.Time {
	return r.NextReleaseDate
}

// GetReleaseSchedule - returns release schedule
func (r *Record) GetReleaseSchedule() string {
	return r.ReleaseSchedule
}

// GetIsFinished - comment
func (r *Record) GetIsFinished() bool {
	return r.IsFinished
}

// UpdateName - edits the title of this record
func (r *Record) UpdateName(_name string) {
	r.Name = _name
}

// UpdateCategoryID - Updates the category this record belongs to
func (r *Record) UpdateCategoryID(_newCatID int) {
	r.CategoryID = _newCatID
}

// UpdateCurrentIssue - edits the current issue of this record
func (r *Record) UpdateCurrentIssue(_ci int) {
	r.CurrentIssue = _ci
}

// UpdateLatestReleased - edits the title of this record
func (r *Record) UpdateLatestReleased(_lr int) {
	r.LatestReleased = _lr
}

// UpdateNextReleaseDate - edits the title of this record
func (r *Record) UpdateNextReleaseDate(_nr time.Time) {
	r.NextReleaseDate = _nr
}

// CalculateAndUpdateNextReleaseDateAndIssue - edits the title of this record
func CalculateAndUpdateNextReleaseDateAndIssue(r *Record) bool {

	// we get rid of all the extra data we dont care about
	y, m, d := time.Now().Date()
	now := time.Date(y, m, d, 0, 0, 0, 0, time.UTC)
	fmt.Printf("next release date: %s\n", r.NextReleaseDate.String())
	fmt.Printf("Now date: %s\n", now.String())
	//fmt.Print("Short Now My Date: \n")
	//fmt.Print(now.String())

	isNowOrNextEqual := r.NextReleaseDate.Equal(now)
	NextLessThanNow := r.NextReleaseDate.Sub(now).Hours()

	if r.IsFinished == false {
		// if today is a new release or if record is way out of date
		if isNowOrNextEqual || NextLessThanNow < 0 { //r.NextReleaseDate.Equal(now) {
			//fmt.Println("\nDate comparison worked!!!!!!!")
			switch sw := r.ReleaseSchedule; sw {
			case "Weekly":
				{
					date := r.NextReleaseDate
					amountToIncrease := 0
					for {
						if date.Sub(now).Hours() >= 0 {
							break
						}
						date = date.AddDate(0, 0, 7)
						amountToIncrease++
					}
					// date := r.NextReleaseDate.AddDate(0, 0, 7)

					// Get difference in days
					//hourDiff := date.Sub(r.NextReleaseDate).Hours()
					//dayDiff := hourDiff / 24
					//amountToIncrease := dayDiff / 7
					r.NextReleaseDate = time.Date(date.Year(), date.Month(), date.Day(), 0, 0, 0, 0, time.UTC)
					fmt.Printf("New next release date for %s is: %s\n", r.Name, r.NextReleaseDate.String())
					r.LatestReleased += int(amountToIncrease) //r.LatestReleased++
					return true
				}
			case "Biweekly":
				{
					date := r.NextReleaseDate
					amountToIncrease := 0
					for {
						h := date.Sub(now).Hours()
						if h >= 0 {
							break
						}
						date = date.AddDate(0, 0, 14)
						amountToIncrease++
					}
					//date := r.NextReleaseDate.AddDate(0, 0, 14)
					// Get difference in days
					//hourDiff := date.Sub(r.NextReleaseDate).Hours()
					//dayDiff := hourDiff / 24
					//amountToIncrease := dayDiff / 14 // two weeks
					r.NextReleaseDate = time.Date(date.Year(), date.Month(), date.Day(), 0, 0, 0, 0, time.UTC)
					fmt.Printf("New next release date for %s is: %s\n", r.Name, r.NextReleaseDate.String())
					r.LatestReleased += amountToIncrease //r.LatestReleased++
					return true
				}
			case "Monthly":
				{
					date := r.NextReleaseDate
					amountToIncrease := 0
					for {
						if date.Sub(now).Hours() >= 0 {
							break
						}
						date = date.AddDate(0, 1, 0)
						amountToIncrease++
					}
					//date := r.NextReleaseDate.AddDate(0, 1, 0)
					// Get difference in days
					//hourDiff := date.Sub(r.NextReleaseDate).Hours()
					//dayDiff := hourDiff / 24
					//amountToIncrease := dayDiff / 31 // one month
					r.NextReleaseDate = time.Date(date.Year(), date.Month(), date.Day(), 0, 0, 0, 0, time.UTC)
					fmt.Printf("New next release date for %s is: %s\n", r.Name, r.NextReleaseDate.String())
					r.LatestReleased += amountToIncrease //r.LatestReleased++
					return true
				}
			}
		} else {
			fmt.Println("\nDate not equal!!!!!!!")
		}
	}
	return false
}

// UpdateGenre - edits the title of this record
func (r Record) UpdateGenre(_g string) {
	r.Genre = _g
}

// UpdatePublisher - edits the title of this record
func (r Record) UpdatePublisher(_p string) {
	r.Publisher = _p
}

// UpdateInitialReleaseDate - Updates the initial release date (dunno why this would be done)
func (r Record) UpdateInitialReleaseDate(_ir time.Time) {
	r.InitialReleaseDate = _ir
}

// UpdateWriter - Updates the writter of this record
func (r Record) UpdateWriter(_w string) {
	r.Writer = _w
}

// UpdateArtist - Updates the artist of this record
func (r Record) UpdateArtist(_a string) {
	r.Artist = _a
}

// UpdateIsFinished - Flips the value of mIsFinished
func (r Record) UpdateIsFinished(_isFinished bool) {
	r.IsFinished = _isFinished
}

// UpdateReleaseSchedule - edits the title of this record
func (r Record) UpdateReleaseSchedule(_rsc string) {
	r.ReleaseSchedule = _rsc
}

func compareMyTimeWithTodaysDate(_myTime time.Time) bool {
	now := time.Now()
	// this is how we discard all the extra date data we dont use
	nowString := fmt.Sprintf("%d-%d-%d", now.Year(), now.Month(), now.Day())
	//fmt.Printf("Now String: %s\n", nowString)
	nowMyDate := Stod(nowString)
	if _myTime.Equal(nowMyDate) {
		//fmt.Printf("next release date: %s\n", _myTime.String())
		//fmt.Printf("Now My Date: %s\n", nowMyDate.String())
		return true
	}
	return false
}

// Stod - String to Date. takes a string in the year-month-day format and turns it into a time.Time type
func Stod(_dateString string) time.Time {
	var date time.Time
	if len(_dateString) > 0 {
		dateComp := strings.Split(_dateString, "-")
		year, _ := strconv.Atoi(dateComp[0])

		intconv, _ := strconv.Atoi(dateComp[1])
		month := time.Month(intconv)

		day, _ := strconv.Atoi(dateComp[2])
		date = time.Date(year, month, day, 0, 0, 0, 0, time.UTC)
	}
	return date
}
