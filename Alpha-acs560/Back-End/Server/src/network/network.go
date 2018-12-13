package network

import (
	"category"
	"encoding/json"
	"fmt"
	"manager"
	"net"
	"strings"
)

const (
	// ConnPort - port
	ConnPort = ":8080"
	// ConnType - type
	ConnType = "tcp"
)

type message struct {
	Operation string `json:"Operation"`
	Object    string `json:"Object"`
	ObjectID  int    `json:"ObjectID"`
	Action    string `json:"Action"`
}

// only needed below for sample processing

var listener net.Listener
var conn net.Conn

// LaunchServer - launches the server
func LaunchServer() {
	//var err error
	//listener, err = net.Listen(Conn_Type, Conn_Port)
	//CheckErrorPanic(err)
	fmt.Println("Listener created!")
	var err error
	listener, err = net.Listen(ConnType, ConnPort)
	CheckErrorPanic(err)

}

// CloseServer - closes the server
func CloseServer() {
	conn.Close()
	listener.Close()
}

// Listen - will listen for messages from client
//			and return what was read back to main
//			so it can act appropriately
func Listen() string {
	fmt.Println("Listening...")
	var err error
	conn, err = listener.Accept()
	CheckErrorPanic(err)
	fmt.Println("Listened!!!")

	ParseMessage()
	return "huzza!"
}

// ParseMessage - determines what to do with the message
func ParseMessage() {
	var parsedMessage message
	decoder := json.NewDecoder(conn)
	fmt.Println("Decoding message:")
	err := decoder.Decode(&parsedMessage)
	CheckErrorJustALittleBitAlarmed(err)
	fmt.Println("Message decoded:")
	fmt.Println(parsedMessage)
	parseAndExecuteOperation(parsedMessage)

}

func updateCategory(_id int, _action string) {

}

func sendAllCategories() {
	var catArray []category.Category
	encoder := json.NewEncoder(conn)

	catArray = manager.GetAllCategories()

	l := len(catArray)
	if l == 0 {
		encoder.Encode("")
	}
	encoder.Encode(catArray)
	fmt.Println("Categories sent:")
	fmt.Println(catArray)
}

func sendAllRecords(_CatID int) {
	//var recArray []record.Record
	encoder := json.NewEncoder(conn)

	recArray := manager.GetAllRecords(_CatID)
	l := len(recArray)
	if l == 0 {
		encoder.Encode("")
	}

	err := encoder.Encode(recArray)
	if err != nil {
		panic(err)
	}
	fmt.Println("Records sent:")
}

// Format - <operation> <object type> <action>
// 			operation:
//					C - create
//					R - read (retrieve)
//					U - update
//					D - delete
//			object type:
//					rec - record
//					cat - category
//					CatAll - all the categories
//					recAll - all the records
func parseAndExecuteOperation(parsedMessage message) {
	fmt.Println("About to parse the message:")
	fmt.Println(parsedMessage)
	obj := strings.ToLower(parsedMessage.Object)
	switch sw := strings.ToLower(parsedMessage.Operation); sw {
	case "c":
		{
			if obj == "cat" {
				newID := manager.AddCategories(parsedMessage.Action)
				SendConfirmationBackWithValue(newID)

			} else if obj == "rec" {
				newID := manager.AddRecord(parsedMessage.Action)
				SendConfirmationBackWithValue(newID)
			}
			break
		}
	case "r":
		{
			if obj == "cat" {
				fmt.Printf("Sending category id:%d...", parsedMessage.ObjectID)

			} else if obj == "rec" {

			} else if obj == "catall" {
				fmt.Println("Sending all the categories...")
				sendAllCategories()
			} else if obj == "recall" {

				fmt.Println("Sending all records...")
				sendAllRecords(parsedMessage.ObjectID)
			}
			break
		}
	case "u":
		{
			if obj == "cat" {
				fmt.Printf("Updating category id:%d...\n", parsedMessage.ObjectID)
				result := manager.UpdateCategory(parsedMessage.ObjectID, parsedMessage.Action)
				SendConfirmationBackEmpty(result)
			} else if obj == "rec" {
				fmt.Printf("Updating record id:%d...\n", parsedMessage.ObjectID)
				result := manager.UpdateRecord(parsedMessage.ObjectID, parsedMessage.Action)
				SendConfirmationBackEmpty(result)
			}
			break
		}
	case "d":
		{
			if obj == "cat" {
				fmt.Printf("Removing category id: %d\n", parsedMessage.ObjectID)
				id := parsedMessage.ObjectID
				removeWork := manager.RemoveCategory(id)
				fmt.Printf("removal of category: %t\n", removeWork)
				SendConfirmationBackEmpty(removeWork)
			} else if obj == "rec" {
				fmt.Printf("Removing record id: %d\n", parsedMessage.ObjectID)
				removeWork := manager.RemoveRecord(parsedMessage.ObjectID)
				SendConfirmationBackEmpty(removeWork)
			}
			break
		}
	}
}

// SendConfirmationBackEmpty - sends ok or fail if action was goo or ba
func SendConfirmationBackEmpty(_iswork bool) {
	encoder := json.NewEncoder(conn)
	if _iswork {
		fmt.Println("OK: Action successful")
		encoder.Encode("ok")
	} else {
		fmt.Println("FAIL: Action Failed")
		encoder.Encode("fail")
	}
}

// SendConfirmationBackWithValue - sends -1 if failed, appropriate number if ok
func SendConfirmationBackWithValue(_isWork int) {
	encoder := json.NewEncoder(conn)
	if _isWork == -1 {
		fmt.Println("FAIL: Action Failed")
	} else {
		fmt.Println("OK: Action successful")
	}
	encoder.Encode(_isWork)
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
