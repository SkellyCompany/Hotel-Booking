Feature: CreateBooking
	A hotel room can be booked for a period (start date – end date) in the future
	provided that it is not already booked for one or more days during the desired period

@mytag
Scenario: The Start date and the End date are before the occupied range
	Given the start date is 2021-11-04
	And the end date is 2021-11-05
	When the method 'CreateBooking' is called
	Then the result should return true

#@mytag
#Scenario: The Start date and the End date are after the occupied range
#	Given the start date is 2022-01-22
#	And the end date is 2022-01-24
#	When the method 'CreateBooking' is called
#	Then the result should return true

#@mytag
#Scenario: The Start date is before and the End date is after the occupied range
#	Given the start date is 2021-11-04
#	And the end date is 2021-11-05
#	When the method 'CreateBooking' is called
#	Then the result should return true
#
#@mytag
#Scenario: The Start date is before and the End date is during the occupied range
#	Given the start date is 2021-11-04
#	And the end date is 2021-11-05
#	When the method 'CreateBooking' is called
#	Then the result should return true
#
#@mytag
#Scenario: The Start date is during and the End date is after the occupied range
#	Given the start date is 2021-11-04
#	And the end date is 2021-11-05
#	When the method 'CreateBooking' is called
#	Then the result should return true
#
#@mytag
#Scenario: The Start date and the End date are during the occupied range
#	Given the start date is 2021-11-04
#	And the end date is 2021-11-05
#	When the method 'CreateBooking' is called
#	Then the result should return true