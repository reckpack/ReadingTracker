package category

import (
	"fmt"
	"record"
)

// Category - category
type Category struct {
	ID            int    `json:"id"`
	Name          string `json:"name"`
	APILink       string `json:"api_link"`
	IndexInEditor int    `json:"IndexInEditor"`
	mRecords      map[int]record.Record
}

// New - Creates new category
func New(_id int, _name string, _ApiLink string, _indexInEditor int) Category {
	c := Category{_id, _name, _ApiLink, _indexInEditor, make(map[int]record.Record)}
	return c
}

// GetAllRecords - returns all the records in the category
//					used for sending records to the client
func (c Category) GetAllRecords() *map[int]record.Record {
	return &c.mRecords
}

// GetName - returns the name
func (c Category) GetName() string {
	return c.Name
}

// GetID - returns the id
func (c Category) GetID() int {
	return c.ID
}

// IsRecordExist - returns true if the id passed in belongs to this category
func (c Category) IsRecordExist(_id int) bool {
	_, ok := c.mRecords[_id]
	return ok
}

// DisplayRecords - Shows the current categories in storage
func (c Category) DisplayRecords() {
	println(c.mRecords)
}

// EditName - Edits title
func (c *Category) EditName(_newName string) {
	c.Name = _newName
}

// EditAPILink - Edits API link
func (c *Category) EditAPILink(_ApiLink string) {
	c.APILink = _ApiLink
}

// EditIndexInEditor - Edits the value that represents a category's index in the front-end
func (c *Category) EditIndexInEditor(_indexInEditor int) {
	c.IndexInEditor = _indexInEditor
}

// UpdateRecord - dsafds
func (c Category) UpdateRecord(_r record.Record) {
	c.mRecords[_r.MID] = _r
}

// GetRecord - retrieves the specified record
func (c Category) GetRecord(_id int) *record.Record {
	rec := c.mRecords[_id]
	return &rec
}

// InsertRecord - Inserts a new record into the records list
func (c Category) InsertRecord(_r record.Record) {

	c.mRecords[_r.MID] = _r
}

// RemoveRecord - Inserts a new record into the records list
func (c Category) RemoveRecord(_id int) {
	_, ok := c.mRecords[_id]
	if ok {
		delete(c.mRecords, _id)
		fmt.Printf("Record at category: %s with ID: %d deleted\n", c.Name, _id)
	}
}
