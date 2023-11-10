import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "simpleDateFormat"
})

export class simpleDateFormat implements PipeTransform {
  transform(value: string): string {

    //  Starts with "04.11.2023 12:43"
    //  First splits the string by blankspace char. and continue with the left part.
    //  Then split up again by '.' so we get three seperate parts.
    var stringSplitted = value.split(" ")[0].split(".");
    //  Format the returning string with '/' as delimiter and only keep last two digits of year
    return stringSplitted[0]+"/"+stringSplitted[1]+"/"+stringSplitted[2].slice(-2);
  }
}
