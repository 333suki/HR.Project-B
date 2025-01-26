// Interface to create a reservation by the user
interface IReservation
{
    string SelectLocation();
    string SelectTimeSlot();
    string SelectAmountOfPeople();
    Reservation CreateReservation();
    string DisplayInfo();
}