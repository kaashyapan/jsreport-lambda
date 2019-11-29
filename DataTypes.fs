namespace ReportMaker

type DataMsg =
    { message: string }

type Address =
    { name: string
      road: string
      country: string }

type Item =
    { name: string
      price: int }

type DataStruct =
    { number: string
      seller: Address
      buyer: Address
      items: List<Item> }
