import os
import pandas as pd

def filterInvalidData(df, currentAnalyzedCol):
    # remove nan and text values
    analyzedFinancialValue = df[currentAnalyzedCol].apply(pd.to_numeric, errors='coerce').dropna()

    # zero represents missing data
    # analyzedFinancialValueWithoutZeros = analyzedFinancialValue[(analyzedFinancialValue!=0) & (analyzedFinancialValue>-3) & (analyzedFinancialValue<3)]
    analyzedFinancialValueWithoutZeros = analyzedFinancialValue[analyzedFinancialValue!=0]

    return analyzedFinancialValueWithoutZeros

def loadDataFramesFromFile():
    col_list = ["ROA2012", "ROA2013", "ROA2014", "ROE2012", "ROE2013", "ROE2014"]
    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/final_output.csv"), usecols=col_list, delimiter=';', encoding='utf8')

    return df