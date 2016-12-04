# What is NaturalShift?
NaturalShift is a .NET library useful to compute workshifts. Computation is based on genetic algoritms. Number of days, shifts and shifters can be configured. The library is released under the terms of AGPL-3.0 license.
# A simple example
As an example, let's take a typical workshift by dividing a day into three eight-hour shifts, to cover the 24 hours. Each day, there are 5 morning shifts, 5 afternoon shifts, 4 overnight shifts: there is a total of 14 shifts to be arranged each day. The scheme is 30 days long. This setting is shown in the picture.
<here the 5-5-4workshift image>
18 employees concur to fill the scheme. Let's suppose that employees cannot work longer than 5 consecutive days. Whenever an employee performs five consecutive working days, she must rest 2 days. The workshift scheme appears as follows.
