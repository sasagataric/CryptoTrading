export interface IInfoDataCards{
    heading:string,
    overlayTarget:string[],
    numberWithSimbol:string,
    percentage?: number | undefined,
    progresBar?:{
        label:string,
        max:number,
        now:number
    } | undefined
}
