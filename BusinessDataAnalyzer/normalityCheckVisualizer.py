import os
import matplotlib.pyplot as plt
import numpy as np

from dataLoader import filterInvalidData, loadDataFramesFromFile

def generateHistogram(df, currentAnalyzedCol, axs, row, col, sampleSize):
    filteredAnalyzedData = filterInvalidData(df, currentAnalyzedCol)

    axs[row, col].hist(filteredAnalyzedData, bins=np.arange(-3, 3, sampleSize))
    axs[row, col].title.set_text(currentAnalyzedCol)

def main():
    df = loadDataFramesFromFile()

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