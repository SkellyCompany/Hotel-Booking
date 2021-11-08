Feature: CreateBooking

@mytag
Scenario: the start date and the end date are before the 
	Given the start date is today + 1 days
	And the end date is today + 2 days
	When the method 'CreateBooking' is called
	Then the result should return true

#
#Scenario: The start date and the end date are before the occupied range
#	Given the start date is today + 1 days
#	And the end date is today + 2 days
#	When the method 'CreateBooking' is called
#	Then the result should return true
#
#	Scenario: The start date and the end date are before the occupied rangezzz
#	Given the start date is today + 1 days
#	And the end date is today + 2 days
#	When the method 'CreateBooking' is called
#	Then the result should return true

#@mytag
#Scenario: The start date and the end date are after the occupied range
#	Given the start date is today + 1 days
#	And the end date is today + 2 days
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