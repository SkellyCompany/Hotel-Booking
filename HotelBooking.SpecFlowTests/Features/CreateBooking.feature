Feature: CreateBooking
	A hotel room can be booked for a period (start date – end date) in the future,
	if it is not already booked for one or more days during the desired period.


@mytag
Scenario: The start date and the end date are before the occupied range
	Given the start date is today + 1 days
	And the end date is today + 2 days
	When the method 'CreateBooking' is called
	Then the result should return true

@mytag
Scenario: The start date and the end date are after the occupied range
	Given the start date is today + 11 days
	And the end date is today + 12 days
	When the method 'CreateBooking' is called
	Then the result should return true

@mytag
Scenario: The start date is before and the end date is after the occupied range
	Given the start date is today + 1 days
	And the end date is today + 12 days
	When the method 'CreateBooking' is called
	Then the result should return false

@mytag
Scenario Outline: The start date is before and end date is during the occupied range
	Given the start date is today + <StartDateDays> days
	And the end date is today + <EndDateDays> days
	When the method 'CreateBooking' is called
	Then the result should return false

	Examples:
	| StartDateDays | EndDateDays		|
	| 1				| 5					|
	| 1				| 10				|

@mytag
Scenario Outline: The start date is during and end date is after the occupied range
	Given the start date is today + <StartDateDays> days
	And the end date is today + <EndDateDays> days
	When the method 'CreateBooking' is called
	Then the result should return false

	Examples:
	| StartDateDays | EndDateDays		|
	| 5				| 12				|
	| 10			| 12				|

@mytag
Scenario Outline: The start date and the end date are during the occupied range
	Given the start date is today + <StartDateDays> days
	And the end date is today + <EndDateDays> days
	When the method 'CreateBooking' is called
	Then the result should return false

	Examples:
	| StartDateDays | EndDateDays		|
	| 5				| 6					|
	| 9				| 10				|
	| 5				| 10				|