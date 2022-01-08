import os
import pandas as pd

from filters import filterOutLayers, filterCompaniesWithOneRecord

def describeDichtomicData(df):
    colToAnalyze = [
        "Zahranicny_vlastnik_FDI",
        "Zahranicny_vlastnik_Majoritny",
        "Vlastnik_PO",
        "Jednoosobova_SRO",
        "Statny_vlastnik",
        "Sekcia"
    ]
    
    for i in colToAnalyze:
        print(df.groupby(i)["ICO"].nunique())
        print("\n")

def describeConcentrationVariables(df):
    colToAnalyze = [
        "Koncentracia_vlastnictva_h3",
        "Koncentracia_vlastnictva_h5",
        "Koncentracia_vlastnictva_t3",
        "Koncentracia_vlastnictva_t5",
    ]

    for i in colToAnalyze:
        print(df[i].describe())
        print("\n")

def describePerformanceVariableWithGrouping(df, performanceVariable):
    colToAnalyze = [
        "Zahranicny_vlastnik_FDI",
        "Zahranicny_vlastnik_Majoritny",
        "Vlastnik_PO",
        "Jednoosobova_SRO",
        "Statny_vlastnik",
        "Sekcia"
    ]
    
    for i in colToAnalyze:
        print(df.groupby(i)[performanceVariable].describe())
        print("\n")

def main(performanceVariable):
    col_list = [
        "ICO",
        "Rok",
        "ROA",
        "ROE",
        "Zahranicny_vlastnik_FDI",
        "Zahranicny_vlastnik_Majoritny",
        "Vlastnik_PO",
        "Koncentracia_vlastnictva_h3",
        "Koncentracia_vlastnictva_h5",
        "Koncentracia_vlastnictva_t3",
        "Koncentracia_vlastnictva_t5",
        "Jednoosobova_SRO",
        "Statny_vlastnik",
        "Sekcia"
    ]
    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/panel_data.csv"), usecols=col_list, delimiter=';', encoding='utf8')

    dfWithoutOutLayer = filterOutLayers(df, performanceVariable)
    dfCleared = filterCompaniesWithOneRecord(dfWithoutOutLayer).reset_index()

    print(dfCleared[performanceVariable].describe())
    print("Total company count", dfCleared["ICO"].nunique())
    print(dfCleared.groupby("Sekcia")[performanceVariable].describe())

    describeConcentrationVariables(dfCleared)
    describeDichtomicData(dfCleared)
    describePerformanceVariableWithGrouping(dfCleared, performanceVariable)

if __name__ == "__main__":
    main("ROA")
