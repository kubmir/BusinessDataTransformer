import numpy as np

def getIQR(x):
    q75, q25 = np.percentile(x, [75 ,25])
    return q75 - q25

def getUpperLowerLimit(iqr, x):
    q1 = np.percentile(x, 25)
    q3 = np.percentile(x, 75)
    
    upperLimit = q3 + 1.5 * iqr
    lowerLimit = q1 - 1.5 * iqr
    
    return lowerLimit, upperLimit

def filterOutLayers(df, currentAnalyzedCol):
    df = df.loc[df[currentAnalyzedCol] != 0]
    iqr = getIQR(df[[currentAnalyzedCol]])
    lowerLimit, upperLimit = getUpperLowerLimit(iqr, df[[currentAnalyzedCol]])

    return df[(df[currentAnalyzedCol] >= lowerLimit) & (df[currentAnalyzedCol] <= upperLimit) & (df["Koncentracia_vlastnictva"] <= 10000) &  (df["Koncentracia_vlastnictva"] >= 0)]