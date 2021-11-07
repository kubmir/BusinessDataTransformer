import os
import pandas as pd

def filterInvalidData(df, currentAnalyzedCol):
    # remove nan and text values
    analyzedFinancialValue = df[currentAnalyzedCol].apply(pd.to_numeric, errors='coerce').dropna()

    # zero represents missing data
    # analyzedFinancialValueWithoutZeros = analyzedFinancialValue[(analyzedFinancialValue!=0) & (analyzedFinancialValue>-0.5) & (analyzedFinancialValue<0.5)]
    analyzedFinancialValueWithoutZeros = analyzedFinancialValue[(analyzedFinancialValue!=0) & (analyzedFinancialValue>-3) & (analyzedFinancialValue<3)]
    # analyzedFinancialValueWithoutZeros = analyzedFinancialValue[analyzedFinancialValue!=0]

    return analyzedFinancialValueWithoutZeros

def loadDataFramesFromFile():
    col_list = ["ROA2012", "ROA2013", "ROA2014", "ROE2012", "ROE2013", "ROE2014"]
    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/final_v2.csv"), usecols=col_list, delimiter=';', encoding='utf8', dtype=float)
    # df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/test.csv"), usecols=col_list, delimiter=';', encoding='utf8', dtype=float)

    print(df)
    return df

def loadDataForOwnerTest(year, analyzedMetric):
    col_list = [
        "{}-1.Majitel-KrajinaPriznak".format(year),
        "{}-1.Majitel-Podiel".format(year),
        "{}-2.Majitel-KrajinaPriznak".format(year),
        "{}-2.Majitel-Podiel".format(year),
        "{}-3.Majitel-KrajinaPriznak".format(year),
        "{}-3.Majitel-Podiel".format(year),
        "{}{}".format(analyzedMetric, year)
    ]

    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/final_v2.csv"), usecols=col_list, delimiter=';', encoding='utf8')

    return df