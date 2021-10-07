import os
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

def generateHistogram(df, currentAnalyzedCol, axs, row, col, sampleSize):
    # remove nan and text values
    analyzedFinancialValue = df[currentAnalyzedCol].apply(pd.to_numeric, errors='coerce').dropna()

    # zero represents missing data
    analyzedFinancialValueWithoutZeros = analyzedFinancialValue[analyzedFinancialValue!=0]

    axs[row, col].hist(analyzedFinancialValueWithoutZeros, bins=np.arange(-3, 3, sampleSize))
    axs[row, col].title.set_text(currentAnalyzedCol)


def main():
    col_list = ["ROA2012", "ROA2013", "ROA2014", "ROE2012", "ROE2013", "ROE2014"]
    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/final_output.csv"), usecols=col_list, delimiter=';', encoding='utf8')

    fig, axs = plt.subplots(2, 3, sharey=True, tight_layout=True)

    generateHistogram(df, "ROA2012", axs, 0, 0, 0.001)
    generateHistogram(df, "ROA2013", axs, 0, 1, 0.001)
    generateHistogram(df, "ROA2014", axs, 0, 2, 0.001)

    generateHistogram(df, "ROE2012", axs, 1, 0, 0.002)
    generateHistogram(df, "ROE2013", axs, 1, 1, 0.002)
    generateHistogram(df, "ROE2014", axs, 1, 2, 0.002)

    plt.ylim(top=800, bottom=0)

    plt.savefig(os.path.expanduser("~/Desktop/Diplomovka_ESF/outputs/histogram_normality.png"))
    plt.show()

if __name__ == "__main__":
    main()